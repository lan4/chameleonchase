﻿using System;
using System.Collections.Generic;
using System.Text;

using FlatRedBall;
using FlatRedBall.Graphics;
using FlatRedBall.Math.Geometry;
using FlatRedBall.Input;


// Be sure to replace:
// 1.  The namespace
// 2.  The class name
// 3.  The constructor (should be the same as the class name)


namespace ChameleonChase
{
    public class CrippleMan : PositionedObject
    {
        #region Fields

        // Here you'd define things that your Entity contains, like Sprites
        // or Circles:
        private Sprite mVisibleRepresentation;
        private Circle mCollision;

        // Keep the ContentManager for easy access:
        string mContentManagerName;

        int railPos;
        bool isJumping;
        float groundLevel;

        #endregion

        #region Properties


        // Here you'd define properties for things
        // you want to give other Entities and game code
        // access to, like your Collision property:
        public Circle Collision
        {
            get { return mCollision; }
        }

        #endregion

        #region Methods

        // Constructor
        public CrippleMan(string contentManagerName)
        {
            // Set the ContentManagerName and call Initialize:
            mContentManagerName = contentManagerName;

            // If you don't want to add to managers, make an overriding constructor
            Initialize(true);
        }

        protected virtual void Initialize(bool addToManagers)
        {
            // Here you can preload any content you will be using
            // like .scnx files or texture files.
            
            if (addToManagers)
            {
                AddToManagers(null);
            }
        }

        public virtual void AddToManagers(Layer layerToAddTo)
        {
            // Add the Entity to the SpriteManager
            // so it gets managed properly (velocity, acceleration, attachments, etc.)
            SpriteManager.AddPositionedObject(this);

            // Here you may want to add your objects to the engine.  Use layerToAddTo
            // when adding if your Entity supports layers.  Make sure to attach things
            // to this if appropriate.
            mVisibleRepresentation = SpriteManager.AddSprite("redball.bmp", mContentManagerName);
            mVisibleRepresentation.AttachTo(this, false);

            mCollision = ShapeManager.AddCircle();
            mCollision.AttachTo(this, false);

            this.XVelocity = 5.0f;

            railPos = 1;
            isJumping = false;

            SpriteManager.Camera.XVelocity = 5.0f;
        }

        public void Move()
        {
            if (InputManager.Keyboard.KeyPushed(Microsoft.Xna.Framework.Input.Keys.Up))
            {
                if (railPos != 2)
                {
                    this.X += 5.0f;
                    this.Y += 5.0f;
                    railPos += 1;
                }
            }
            else if (InputManager.Keyboard.KeyPushed(Microsoft.Xna.Framework.Input.Keys.Down))
            {
                if (railPos != 0)
                {
                    this.X -= 5.0f;
                    this.Y -= 5.0f;
                    railPos -= 1;
                }
            }

            groundLevel = this.Y;
        }

        public void Jump()
        {
            if (InputManager.Keyboard.KeyPushed(Microsoft.Xna.Framework.Input.Keys.Space))
            {
                isJumping = true;
                this.YVelocity = 10.0f;
                this.YAcceleration = -5.0f;
            }
            else if (isJumping)
            {
                if (this.Y <= groundLevel)
                {
                    this.Y = groundLevel;
                    this.YVelocity = 0.0f;
                    this.YAcceleration = 0.0f;
                    isJumping = false;
                }
            }
        }

        public virtual void Activity()
        {
            // This code should do things like set Animations, respond to input, and so on.
            if (!isJumping)
                Move();
            Jump();
        }

        public virtual void Destroy()
        {
            // Remove self from the SpriteManager:
            SpriteManager.RemovePositionedObject(this);

            // Remove any other objects you've created:
            SpriteManager.RemoveSprite(mVisibleRepresentation);
            ShapeManager.Remove(mCollision);
        }

        #endregion
    }
}