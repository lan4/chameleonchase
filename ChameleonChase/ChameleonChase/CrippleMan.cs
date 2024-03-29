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

        bool isDamaged;
        bool isBoosting;
        int hp;

        double damageTime;
        double boostTime;

        #endregion

        #region Properties


        // Here you'd define properties for things
        // you want to give other Entities and game code
        // access to, like your Collision property:
        public Circle Collision
        {
            get { return mCollision; }
        }

        public int RailPosition
        {
            get { return railPos; }
        }

        public bool IsDamaged
        {
            get { return isDamaged; }
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
            mVisibleRepresentation = SpriteManager.AddSprite("GuyInChair.png", mContentManagerName);

            float texturePixelWidth = mVisibleRepresentation.Texture.Width;
            float texturePixelHeight = mVisibleRepresentation.Texture.Height;

            float pixelsPerUnit = SpriteManager.Camera.PixelsPerUnitAt(mVisibleRepresentation.Z);

            mVisibleRepresentation.ScaleX = .25f * texturePixelWidth / pixelsPerUnit;
            mVisibleRepresentation.ScaleY = .25f * texturePixelHeight / pixelsPerUnit;

            mVisibleRepresentation.AttachTo(this, false);
            mVisibleRepresentation.RelativeY = 1.0f;

            mCollision = ShapeManager.AddCircle();
            mCollision.AttachTo(this, false);
            mCollision.Visible = false;

            this.XVelocity = 25.0f;

            railPos = 1;
            isJumping = false;
            isDamaged = false;

            SpriteManager.Camera.XVelocity = 25.0f;

            Sprite background = SpriteManager.AddSprite("TestBackground.png", mContentManagerName);

            texturePixelWidth = background.Texture.Width;
            texturePixelHeight = background.Texture.Height;

            pixelsPerUnit = SpriteManager.Camera.PixelsPerUnitAt(background.Z);

            background.ScaleX = .5f * texturePixelWidth / pixelsPerUnit;
            background.ScaleY = .5f * texturePixelHeight / pixelsPerUnit;

            background.AttachTo(SpriteManager.Camera, false);
            background.RelativeZ = -50.0f;
        }

        public void Move()
        {
            if (InputManager.Keyboard.KeyPushed(Microsoft.Xna.Framework.Input.Keys.Up) || 
                InputManager.Keyboard.KeyPushed(Microsoft.Xna.Framework.Input.Keys.W))
            {
                if (railPos != 2 && railPos + 1 != Chameleon.TongueRail)
                {
                    this.X += 5.0f;
                    this.Y += 10.0f;
                    railPos += 1;
                }
            }
            else if (InputManager.Keyboard.KeyPushed(Microsoft.Xna.Framework.Input.Keys.Down) || 
                     InputManager.Keyboard.KeyPushed(Microsoft.Xna.Framework.Input.Keys.S))
            {
                if (railPos != 0 && railPos - 1 != Chameleon.TongueRail)
                {
                    this.X -= 5.0f;
                    this.Y -= 10.0f;
                    railPos -= 1;
                }
            }

            if (InputManager.Keyboard.KeyDown(Microsoft.Xna.Framework.Input.Keys.Right) || 
                InputManager.Keyboard.KeyDown(Microsoft.Xna.Framework.Input.Keys.D))
            {
                if (isBoosting)
                    this.XVelocity = 47.0f + Screens.GameScreen.SpeedMod;
                else if (isDamaged)
                    this.XVelocity = 23.0f;
                else
                    this.XVelocity = 45.0f + Screens.GameScreen.SpeedMod;
            }
            else if (InputManager.Keyboard.KeyDown(Microsoft.Xna.Framework.Input.Keys.Left) || 
                     InputManager.Keyboard.KeyDown(Microsoft.Xna.Framework.Input.Keys.A))
            {
                if (isBoosting)
                    this.XVelocity = 7.0f + Screens.GameScreen.SpeedMod;
                else if (isDamaged)
                    this.XVelocity = 23.0f;
                else
                    this.XVelocity = 5.0f + Screens.GameScreen.SpeedMod;
            }
            else
            {
                if (isBoosting)
                    this.XVelocity = 27.0f + Screens.GameScreen.SpeedMod;
                else if (isDamaged)
                    this.XVelocity = 23.0f;
                else
                    this.XVelocity = 25.0f + Screens.GameScreen.SpeedMod;
            }

            groundLevel = this.Y;
        }

        public void Jump()
        {
            if (InputManager.Keyboard.KeyPushed(Microsoft.Xna.Framework.Input.Keys.Space) && !isJumping)
            {
                isJumping = true;
                this.YVelocity = 25.0f;
                this.YAcceleration = -50.0f;
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

        public void KeepOnScreen()
        {
            float posDiff = this.X - SpriteManager.Camera.X;

            if (posDiff > 20.0f || posDiff < -20.0f)
            {
                if (isBoosting)
                    this.XVelocity = 27.0f + Screens.GameScreen.SpeedMod;
                else if (isDamaged)
                    this.XVelocity = 23.0f;
                else
                    this.XVelocity = 25.0f + Screens.GameScreen.SpeedMod;

                if (posDiff > 0.0f)
                    this.X -= 0.5f;
                else if (posDiff < 0.0f)
                    this.X += 0.5f;
            }
        }

        public void OnHit()
        {
            if (!isDamaged)
            {
                if (isBoosting)
                {
                    isBoosting = false;
                }
                else
                {
                    isDamaged = true;
                    damageTime = TimeManager.CurrentTime + 1;
                }
            }
        }

        public void OnLicked()
        {
            if (!isDamaged)
            {
                if (isBoosting)
                {
                    isBoosting = false;
                }
                else
                {
                    isDamaged = true;
                    damageTime = TimeManager.CurrentTime + 1;
                }

                if (railPos == 0)
                {
                    this.X += 5.0f;
                    this.Y += 10.0f;
                    railPos += 1;
                }
                else if (railPos == 1)
                {
                    float posDiff = this.X - SpriteManager.Camera.X;

                    if (posDiff > 8.0f)
                    {
                        this.X -= 5.0f;
                        this.Y -= 10.0f;
                        railPos -= 1;
                    }
                    else
                    {
                        this.X += 5.0f;
                        this.Y += 10.0f;
                        railPos += 1;
                    }
                }
                else
                {
                    this.X -= 5.0f;
                    this.Y -= 10.0f;
                    railPos -= 1;
                }

                this.Y = groundLevel;
                this.YVelocity = 0.0f;
                this.YAcceleration = 0.0f;
                isJumping = false;
            }
        }

        public void OnBoost()
        {
            if (isDamaged)
            {
                isDamaged = false;
                mVisibleRepresentation.Visible = true;
            }
            else
            {
                isBoosting = true;
                boostTime = TimeManager.CurrentTime + 2;
            }
        }

        public void FlashSprite()
        {
            mVisibleRepresentation.Visible = !mVisibleRepresentation.Visible;
        }

        public virtual void Activity()
        {
            // This code should do things like set Animations, respond to input, and so on.

            if (isDamaged)
            {
                FlashSprite();
            }

            if (TimeManager.CurrentTime > boostTime)
            {
                isBoosting = false;
            }

            if (TimeManager.CurrentTime > damageTime)
            {
                isDamaged = false;
                mVisibleRepresentation.Visible = true;
            }

            if (!isJumping)
            {
                Move();
            }

            Jump();

            SpriteManager.Camera.XVelocity = 25.0f + Screens.GameScreen.SpeedMod;

            KeepOnScreen();
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
