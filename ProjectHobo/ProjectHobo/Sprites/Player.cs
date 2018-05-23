using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProjectHobo.Models;

namespace ProjectHobo.Sprites
{
  public class Player : Sprite
  {
    private bool _firstPass = true;

    private KeyboardState _currentKey;

    private KeyboardState _previousKey;

    private bool _onGround = false;
    private bool _isSlidingOnLeft = false;
    private bool _isSlidingOnRight = false;

    public Vector2 Velocity;

    public Player(Texture2D texture)
      : base(texture)
    {
    }

    public Player(Dictionary<string, Animation> animations) : base(animations)
    {
    }

    public override void Update(GameTime gameTime)
    {
      if (_firstPass)
      {
        _firstPass = false;

        Body.OnCollision += Body_OnCollision;
        Body.OnSeparation += Body_OnSeparation;
      }

      _previousKey = _currentKey;
      _currentKey = Keyboard.GetState();

      var speed = Density / 5;

      if (_currentKey.IsKeyDown(Keys.A))
      {
        Body.ApplyLinearImpulse(new Vector2(-speed, 0));

      }
      else if (_currentKey.IsKeyDown(Keys.D))
      {
        Body.ApplyLinearImpulse(new Vector2(speed, 0));
      }

      if (_previousKey.IsKeyUp(Keys.Space) &&
          _currentKey.IsKeyDown(Keys.Space))
      {
        Console.WriteLine("Jumping");

        if (_onGround)
        {
          Body.ApplyLinearImpulse(new Vector2(0, Density * 10f));
          _onGround = false;
        }
        else if (_isSlidingOnLeft)
        {
          Body.LinearVelocity = new Vector2(0, 0);
          Body.ApplyLinearImpulse(new Vector2(Density * 6f, Density * 8f));
        }
        else if (_isSlidingOnRight)
        {
          Body.LinearVelocity = new Vector2(0, 0);
          Body.ApplyLinearImpulse(new Vector2(-(Density * 6f), Density * 8f));
        }
      }
    }

    private void Body_OnSeparation(tainicom.Aether.Physics2D.Dynamics.Fixture sender, tainicom.Aether.Physics2D.Dynamics.Fixture other, tainicom.Aether.Physics2D.Dynamics.Contacts.Contact contact)
    {
      if (other.CollisionCategories == tainicom.Aether.Physics2D.Dynamics.Category.Cat2)
      {
        _isSlidingOnLeft = false;
        _isSlidingOnRight = false;
      }
    }

    private bool Body_OnCollision(tainicom.Aether.Physics2D.Dynamics.Fixture sender, tainicom.Aether.Physics2D.Dynamics.Fixture other, tainicom.Aether.Physics2D.Dynamics.Contacts.Contact contact)
    {
      if (other.CollisionCategories == tainicom.Aether.Physics2D.Dynamics.Category.Cat1)
        _onGround = true;

      if (other.CollisionCategories == tainicom.Aether.Physics2D.Dynamics.Category.Cat2)
      {
        if (other.Body.GetTransform().p.X < sender.Body.GetTransform().p.X)
          _isSlidingOnLeft = true;
        else
          _isSlidingOnRight = true;
      }

      Console.WriteLine("Colliding");

      return true;
    }

    public override void PostUpdate(GameTime gameTime)
    {

    }

    public override void OnCollide(Sprite sprite)
    {

    }

    protected override void SetAnimation(GameTime gameTime)
    {
      if (_animationManager == null)
        return;

      if (Body.LinearVelocity.X < 0)
        _animationManager.Flipped = true;
      else _animationManager.Flipped = false;

      if (_onGround)
      {
        if (Body.LinearVelocity.X != 0)
        {
          _animationManager.Play(_animations["Running"]);
        }
        else
        {
          _animationManager.Play(_animations["Idle"]);
        }
      }
      else if (_isSlidingOnLeft)
      {
        _animationManager.Flipped = true;
        _animationManager.Play(_animations["Sliding"]);
      }
      else if (_isSlidingOnRight)
      {
        _animationManager.Flipped = false;
        _animationManager.Play(_animations["Sliding"]);
      }
      else
      {
        if (Body.LinearVelocity.Y < 0)
        {
          _animationManager.Play(_animations["Falling"]);
        }
        else if (Body.LinearVelocity.Y > 0)
        {
          _animationManager.Play(_animations["Jumping"]);
        }
      }

      _animationManager.Update(gameTime);

    }
  }
}
