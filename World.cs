using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Raylib_cs;

namespace meteor_escape;

public class World
{
    private LinkedList<Sprite> _sprites = new();
    private QuadTree _currentQTree;

    public void AddSprite(Sprite sprite) => _sprites.AddLast(sprite);

    public void Update()
    {
        _currentQTree = new QuadTree(new RectangleF(0, 0, Raylib.GetScreenWidth(), Raylib.GetScreenHeight()));
        foreach (var sprite in _sprites)
        {
            sprite.Update();
            _currentQTree.Insert(sprite);
        }

        /// O(n²) algorithm on 1000 object (very slow) 24 FPS
        // foreach (var sprite in _sprites)
        // {
        //     foreach (var other in _sprites)
        //     {
        //         if (other != sprite && sprite.IsCollidingWith(other))
        //         {
        //             sprite.OnCollide(sprite);
        //         }
        //     }
        // }

        /// O(k) algorithm on 1000 object (fast) 60 FPS
        foreach (var sprite in _currentQTree)
        {
            RectangleF range = new RectangleF(sprite.Pos.X, sprite.Pos.Y, (float)(sprite.Rect.Width * 3), (float)(sprite.Rect.Height * 3));
            range.X -= range.Width / 2;
            range.Y -= range.Height / 2;
            List<Sprite> others = _currentQTree.Query(range);
            foreach (var other in others)
            {
                if (other != sprite && sprite.IsCollidingWith(other))
                {
                    sprite.OnCollide(sprite);
                }
            }
        }
    }

    public void Draw()
    {
        foreach (var sprite in _sprites)
        {
            sprite.Draw();
        }
        _currentQTree.Draw();
    }
}

class QuadTree : IEnumerable<Sprite>
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
    private List<Sprite> _elements = new();
    private const uint _maxDepth = 5;
    private readonly uint _depth;
    private readonly uint _cap;
    private readonly RectangleF _bounds;
    private QuadTree _a = null;
    private QuadTree _b = null;
    private QuadTree _c = null;
    private QuadTree _d = null;

    public RectangleF Bounds { get => _bounds; }
    public bool Devided { get => (_a != null) && (_b != null) && (_c != null) && (_d != null); }

    public QuadTree(RectangleF bounds)
        : this(bounds, 3, 0)
    { }

    public QuadTree(RectangleF bounds, uint cap, uint depth)
    {
        this._bounds = bounds;
        this._cap = cap;
        this._depth = depth;
    }

    public List<Sprite> Query(RectangleF range, List<Sprite> found = null)
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
            foreach (var sprite in _elements)
            {
                if (range.IntersectsWith(sprite.Rect))
                {
                    found.Add(sprite);
                }
            }
        }
        return found;
    }

    public bool Insert(Sprite sprite)
    {
        if (!Bounds.IntersectsWith(sprite.Rect))
        {
            return false;
        }

        if (Devided)
        {
            return _a.Insert(sprite)
                || _b.Insert(sprite)
                || _c.Insert(sprite)
                || _d.Insert(sprite);
        }

        _elements.Add(sprite);

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

        foreach (Sprite element in _elements)
        {
            if (_a.Insert(element) || _b.Insert(element) || _c.Insert(element) || _d.Insert(element)) { }
        }

        _elements.Clear();
    }

    public IEnumerator<Sprite> GetEnumerator()
    {
        foreach (var sprite in _elements)
            yield return sprite;

        if (Devided)
        {
            foreach (Sprite sprite in _a)
                yield return sprite;
            foreach (Sprite sprite in _b)
                yield return sprite;
            foreach (Sprite sprite in _c)
                yield return sprite;
            foreach (Sprite sprite in _d)
                yield return sprite;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
