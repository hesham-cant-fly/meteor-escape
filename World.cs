using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.Xna.Framework;

namespace meteor_escape;

public class World
{
    private QuadTree _sprites = new QuadTree(
        new RectangleF(
            -0, -0,
            (float)(Globals.graphicsDevice.Viewport.Bounds.Width * 2.4),
            (float)(Globals.graphicsDevice.Viewport.Bounds.Height * 2.1)
        )
    );

    public void AddSprite(Sprite sprite) => _sprites.Insert(sprite);

    public void Step(GameTime gameTime)
    {
        foreach (var sprite in _sprites)
            sprite.Update(gameTime);
    }

    public void Draw(GameTime gameTime)
    {
        foreach (var sprite in _sprites)
        {
            sprite.Draw(gameTime);
        }
        _sprites.Draw();
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
    private const uint _maxDepth = 6;
    private readonly uint _depth;
    private readonly uint _cap;
    private readonly RectangleF _bounds;
    private bool _devided = false;
    private QuadTree? _a = null;
    private QuadTree? _b = null;
    private QuadTree? _c = null;
    private QuadTree? _d = null;

    public RectangleF Bounds { get => _bounds; }

    public QuadTree(RectangleF bounds)
        : this(bounds, 2, 0)
    { }

    public QuadTree(RectangleF bounds, uint cap, uint depth)
    {
        this._bounds = bounds;
        this._cap = cap;
        this._depth = depth;
    }

    public bool Insert(Sprite sprite)
    {
        if (!Bounds.Contains(sprite.Pos))
        {
            return false;
        }

        if (_devided)
        {
            return _a.Insert(sprite) || _b.Insert(sprite) || _c.Insert(sprite) || _d.Insert(sprite);
        }

        _elements.Add(sprite);

        if (_elements.Count > _cap && _depth < _maxDepth)
        {
            if (!_devided)
            {
                Split();
            }
        }
        return true;
    }

    public void Draw()
    {
        Globals.spriteBatch.DrawString(
            Globals.hudFont,
            $"{_depth}",
            new Vector2(Bounds.X + Bounds.Width / 2, Bounds.Y + Bounds.Height / 2),
            Microsoft.Xna.Framework.Color.Blue
        );
        Globals.DrawOutlineRectangle(
            new Microsoft.Xna.Framework.Rectangle((int)Bounds.X, (int)Bounds.Y, (int)Bounds.Width, (int)Bounds.Height),
            Microsoft.Xna.Framework.Color.White,
            1
        );
        if (_devided)
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
        _devided = true;
    }

    public IEnumerator<Sprite> GetEnumerator()
    {
        foreach (var sprite in _elements)
            yield return sprite;

        if (_devided)
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
