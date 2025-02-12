using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using Raylib_cs;

namespace meteor_escape;

public interface IProcessable
{
    public void Update();
    public void Draw();
}

public class Entity
{
    public Sprite UserData;
    public bool Disabled { get; private set; }

    public Entity(Sprite userData)
    {
        this.UserData = userData;
        this.Disabled = false;
    }

    public void Disable()
    {
        Disabled = true;
    }

    public void Update()
    {
        UserData.Update();
    }

    public void Draw()
    {
        UserData.Draw();
    }

    public override string ToString()
    {
        return $"Entity({Disabled})";
    }
}
public class World
{
    private List<Entity> _enemies = new();
    private List<Entity> _bullets = new();
    public Player Player;
    private QuadTree _currentQTree;

    public World(Player player)
    {
        this.Player = player;
    }

    public void AddEnemie(Sprite enemie) => _enemies.Add(new(enemie));
    public void AddBullet(Sprite bullet) => _bullets.Add(new(bullet));

    public void Update()
    {
        _currentQTree = new QuadTree(new RectangleF(0, 0, Raylib.GetScreenWidth(), Raylib.GetScreenHeight()));
        Player.Update();
        UpdateEnemies();
        UpdateBullets();

        CheckPlayerCollision();
        CheckBulletsCollision();
    }

    private void UpdateEnemies()
    {
        for (int i = 0; i < _enemies.Count; i++)
        {
            Entity entity = _enemies[i];
            if (entity.Disabled)
                continue;
            entity.Update();
            if (!_currentQTree.Insert(entity.UserData, i))
                this.RemoveEnemieAt(i);
        }
    }

    private void UpdateBullets()
    {
        for (int i = 0; i < _bullets.Count; i++)
        {
            Entity entity = _bullets[i];
            if (entity.Disabled)
                continue;
            entity.Update();
            if (!_currentQTree.Insert(entity.UserData, i))
                this.RemoveBulletAt(i);
        }
    }

    public void Draw()
    {
        Player.Draw();
        DrawEnemies();
        DrawBullets();
        _currentQTree.Draw();
    }

    private void DrawEnemies()
    {
        foreach (var entity in _enemies)
        {
            if (entity.Disabled) continue;
            entity.Draw();
        }
    }

    private void DrawBullets()
    {
        foreach (var entity in _bullets)
        {
            if (entity.Disabled) continue;
            entity.Draw();
        }
    }

    public void PostProcess()
    {
    }

    public void RemoveEnemieAt(int i)
    {
        Debug.Assert(i >= 0, $"`i` is less than 0: '{i}'");
        Debug.Assert(i < _enemies.Count, $"`i`: '{i}' more than `enemies.Count`: '{_enemies.Count}'");

        _enemies.RemoveAt(i);
    }

    public void RemoveBulletAt(int i)
    {
        Debug.Assert(i >= 0, $"`i` is less than 0: '{i}'");
        Debug.Assert(i < _bullets.Count, $"`i`: '{i}' more than `bullets.Count`: '{_bullets.Count}'");

        _bullets.RemoveAt(i);
    }

    private void CheckPlayerCollision()
    {
        RectangleF range = Player.HitBox;
        range.Width *= 3;
        range.Height *= 3;
        range.X -= range.Width / 2;
        range.Y -= range.Height / 2;

        var enemies = _currentQTree.Query(range);
        foreach (var other in enemies)
        {
            if (other.Sprite.Kind != SpriteKind.Player && other.Sprite.Kind != SpriteKind.Bullet && Player.IsCollidingWith(other.Sprite))
            {
                Player.OnCollide(-1, other.Index, other.Sprite);
                other.Sprite.OnCollide(other.Index, -1, Player);
            }
        }
    }

    private void CheckBulletsCollision()
    {
        for (int i = 0; i < _bullets.Count; i++)
        {
            var bulletEntity = _bullets[i];
            var bullet = bulletEntity.UserData;
            RectangleF range = bullet.HitBox;
            range.Width *= 3;
            range.Height *= 3;
            range.X -= range.Width / 2;
            range.Y -= range.Height / 2;

            var enemies = _currentQTree.Query(range);
            foreach (var other in enemies)
            {
                if (other.Sprite.Kind != SpriteKind.Bullet && bullet.IsCollidingWith(other.Sprite))
                {
                    bullet.OnCollide(i, other.Index, other.Sprite);
                    other.Sprite.OnCollide(other.Index, i, bullet);
                }
            }
        }
    }
}

class QuadTree : IEnumerable<QuadTree.Leaf>
{
    // +------+------+
    // |      |      |
    // |  A   |  B   |
    // |      |      |
    // +------+------+
    // |      |      |
    // |  C   |  D   |
    // |      |      |
    // +------+------+
    private const uint _maxDepth = 10;
    private QuadTree _a = null;
    private QuadTree _b = null;
    private QuadTree _c = null;
    private QuadTree _d = null;
    private List<Leaf> _elements = new();
    private readonly RectangleF _bounds;
    private readonly uint _depth;
    private readonly uint _cap;

    public RectangleF Bounds { get => _bounds; }
    public bool Devided { get => (_a != null) && (_b != null) && (_c != null) && (_d != null); }

    public struct Leaf(Sprite sprite, int index)
    {
        public Sprite Sprite { get; set; } = sprite;
        public int Index { get; set; } = index;
    }

    public QuadTree(RectangleF bounds)
        : this(bounds, 3, 0)
    { }

    public QuadTree(RectangleF bounds, uint cap, uint depth)
    {
        this._bounds = bounds;
        this._cap = cap;
        this._depth = depth;
    }

    public List<Leaf> Query(RectangleF range, List<Leaf> found = null)
    {
        if (found == null)
        {
            found = new();
        }

        if (!_bounds.IntersectsWith(range))
        {
            return found;
        }

        if (Devided)
        {
            if (_a._bounds.IntersectsWith(range)) _a.Query(range, found);
            if (_b._bounds.IntersectsWith(range)) _b.Query(range, found);
            if (_c._bounds.IntersectsWith(range)) _c.Query(range, found);
            if (_d._bounds.IntersectsWith(range)) _d.Query(range, found);
        }
        else
        {
            foreach (var leaf in _elements)
            {
                if (range.IntersectsWith(leaf.Sprite.HitBox))
                {
                    found.Add(leaf);
                }
            }
        }
        return found;
    }

    public bool Insert(Leaf leaf)
    {
        return Insert(leaf.Sprite, leaf.Index);
    }

    public bool Insert(Sprite sprite, int index)
    {
        if (!Bounds.IntersectsWith(sprite.HitBox))
        {
            return false;
        }

        if (Devided)
        {
            return _a.Insert(sprite, index)
                || _b.Insert(sprite, index)
                || _c.Insert(sprite, index)
                || _d.Insert(sprite, index);
        }

        _elements.Add(new Leaf(sprite, index));

        if (_elements.Count > _cap && _depth < _maxDepth)
        {
            if (!Devided)
            {
                Split();
            }
        }
        return true;
    }

    public void Draw()
    {
        Raylib.DrawText(
            $"{_depth}",
            (int)(Bounds.X + Bounds.Width / 2) - 4, (int)(Bounds.Y + Bounds.Height / 2) - 7,
            15, Raylib_cs.Color.DarkBrown
        );
        Raylib.DrawRectangleLines(
            (int)Bounds.X, (int)Bounds.Y,
            (int)Bounds.Width, (int)Bounds.Height,
            Raylib_cs.Color.Black
        );
        if (Devided)
        {
            _a.Draw();
            _b.Draw();
            _c.Draw();
            _d.Draw();
        }
    }

    private void Split()
    {
        float halfWidth = _bounds.Width / 2,
              halfHeight = _bounds.Height / 2;

        _a = new QuadTree(new RectangleF(_bounds.X, _bounds.Y, halfWidth, halfHeight), _cap, _depth + 1);
        _b = new QuadTree(new RectangleF(_bounds.X + halfWidth, _bounds.Y, halfWidth, halfHeight), _cap, _depth + 1);
        _c = new QuadTree(new RectangleF(_bounds.X, _bounds.Y + halfHeight, halfWidth, halfHeight), _cap, _depth + 1);
        _d = new QuadTree(new RectangleF(_bounds.X + halfWidth, _bounds.Y + halfHeight, halfWidth, halfHeight), _cap, _depth + 1);

        foreach (var leaf in _elements)
        {
            if (_a.Insert(leaf) || _b.Insert(leaf) || _c.Insert(leaf) || _d.Insert(leaf)) { }
        }

        _elements.Clear();
    }

    public IEnumerator<Leaf> GetEnumerator()
    {
        foreach (var leaf in _elements)
            yield return leaf;

        if (Devided)
        {
            foreach (var leaf in _a)
                yield return leaf;
            foreach (var leaf in _b)
                yield return leaf;
            foreach (var leaf in _c)
                yield return leaf;
            foreach (var leaf in _d)
                yield return leaf;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
