﻿using System;
using System.Collections.Generic;
using System.Text;

using FlatRedBall;
using FlatRedBall.Graphics;
using FlatRedBall.Math.Geometry;


// Be sure to replace:
// 1.  The namespace
// 2.  The class name
// 3.  The constructor (should be the same as the class name)


namespace ChameleonChase
{
    public class Chameleon : PositionedObject
    {
        #region Constants
        private const float DEFAULT_CHAMEL_VELOCITY = 25.0f;
        private const float DEFAULT_CHAMEL_POS = -10.0f;
        
        private const float LASER_VELOCITY = 20.0f;
        private const float LASER_TRIGGER_DISTANCE = 300.0f;

        private const float TONGUE_VELOCITY = 15.0f;
        private const float TONGUE_TRIGGER_DISTANCE = 500.0f;
        private const float TONGUE_START = 5.0f;

        private const float RAIL_DISTANCE = 10.0f;

        #endregion

        #region Fields

        // Here you'd define things that your Entity contains, like Sprites
        // or Circles:
        private Sprite mVisibleRepresentation;
        private Sprite head1;
        private Sprite head2;
        private Sprite head3;

        private Sprite heado1;
        private Sprite heado2;
        private Sprite heado3;

        //private Circle mCollision;
        private Polygon mCollision;

        private Circle mLaser;
        private AxisAlignedRectangle mTongue;

        private float mLastLaserPos;
        private bool mIsLasering;
        private int mLaserRail;

        private float mLastTonguePos;
        private bool mIsLicking;
        private int mTongueRail;

        // Keep the ContentManager for easy access:
        string mContentManagerName;

        #endregion

        #region Properties


        // Here you'd define properties for things
        // you want to give other Entities and game code
        // access to, like your Collision property:
        public Polygon Collision
        {
            get { return mCollision; }
        }

        public Circle Laser
        {
            get { return mLaser; }
        }

        public AxisAlignedRectangle Tongue
        {
            get { return mTongue; }
        }

        public static int TongueRail;

        #endregion

        #region Methods

        // Constructor
        public Chameleon(string contentManagerName)
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
            

            head1 = SpriteManager.AddSprite("Cham_Closed.png", mContentManagerName);

            float texturePixelWidth = head1.Texture.Width;
            float texturePixelHeight = head1.Texture.Height;

            float pixelsPerUnit = SpriteManager.Camera.PixelsPerUnitAt(head1.Z);

            head1.ScaleX = .5f * texturePixelWidth / pixelsPerUnit;
            head1.ScaleY = .5f * texturePixelHeight / pixelsPerUnit;

            head1.AttachTo(this, false);

            head2 = SpriteManager.AddSprite("Cham_Closed.png", mContentManagerName);

            texturePixelWidth = head2.Texture.Width;
            texturePixelHeight = head2.Texture.Height;

            pixelsPerUnit = SpriteManager.Camera.PixelsPerUnitAt(head2.Z);

            head2.ScaleX = .5f * texturePixelWidth / pixelsPerUnit;
            head2.ScaleY = .5f * texturePixelHeight / pixelsPerUnit;

            head2.AttachTo(this, false);
            head2.RelativeX = 5.0f;
            head2.RelativeY = 10.0f;

            head3 = SpriteManager.AddSprite("Cham_Closed.png", mContentManagerName);

            texturePixelWidth = head3.Texture.Width;
            texturePixelHeight = head3.Texture.Height;

            pixelsPerUnit = SpriteManager.Camera.PixelsPerUnitAt(head3.Z);

            head3.ScaleX = .5f * texturePixelWidth / pixelsPerUnit;
            head3.ScaleY = .5f * texturePixelHeight / pixelsPerUnit;

            head3.AttachTo(this, false);
            head3.RelativeX = -5.0f;
            head3.RelativeY = -10.0f;

            heado1 = SpriteManager.AddSprite("Cham_Open.png", mContentManagerName);

            texturePixelWidth = heado1.Texture.Width;
            texturePixelHeight = heado1.Texture.Height;

            pixelsPerUnit = SpriteManager.Camera.PixelsPerUnitAt(heado1.Z);

            heado1.ScaleX = .5f * texturePixelWidth / pixelsPerUnit;
            heado1.ScaleY = .5f * texturePixelHeight / pixelsPerUnit;

            heado1.AttachTo(this, false);
            heado1.Visible = false;

            heado2 = SpriteManager.AddSprite("Cham_Open.png", mContentManagerName);

            texturePixelWidth = heado2.Texture.Width;
            texturePixelHeight = heado2.Texture.Height;

            pixelsPerUnit = SpriteManager.Camera.PixelsPerUnitAt(heado2.Z);

            heado2.ScaleX = .5f * texturePixelWidth / pixelsPerUnit;
            heado2.ScaleY = .5f * texturePixelHeight / pixelsPerUnit;

            heado2.AttachTo(this, false);
            heado2.RelativeX = 5.0f;
            heado2.RelativeY = 10.0f;
            heado2.Visible = false;

            heado3 = SpriteManager.AddSprite("Cham_Open.png", mContentManagerName);

            texturePixelWidth = heado3.Texture.Width;
            texturePixelHeight = heado3.Texture.Height;

            pixelsPerUnit = SpriteManager.Camera.PixelsPerUnitAt(heado3.Z);

            heado3.ScaleX = .5f * texturePixelWidth / pixelsPerUnit;
            heado3.ScaleY = .5f * texturePixelHeight / pixelsPerUnit;

            heado3.AttachTo(this, false);
            heado3.RelativeX = -5.0f;
            heado3.RelativeY = -10.0f;
            heado3.Visible = false;

            mVisibleRepresentation = SpriteManager.AddSprite("TankBody.png", mContentManagerName);

            texturePixelWidth = mVisibleRepresentation.Texture.Width;
            texturePixelHeight = mVisibleRepresentation.Texture.Height;

            pixelsPerUnit = SpriteManager.Camera.PixelsPerUnitAt(mVisibleRepresentation.Z);

            mVisibleRepresentation.ScaleX = .25f * texturePixelWidth / pixelsPerUnit;
            mVisibleRepresentation.ScaleY = .5f * texturePixelHeight / pixelsPerUnit;

            mVisibleRepresentation.AttachTo(this, false);
            mVisibleRepresentation.RelativeX = -20.0f;

            //mCollision = ShapeManager.AddCircle();
            //mCollision.AttachTo(this, false);

            Point[] pointArray = 
            {
               new Point(-20,  10), // top left
               new Point(  5,  10), // top right
               new Point( -5, -10), // bottom right
               new Point(-20, -10), // bottom left
               new Point(-20,  10)  // repeat top left to close Polygon
            };

            mCollision = ShapeManager.AddPolygon();
            mCollision.AttachTo(this, false);
            mCollision.Points = pointArray;
            this.mVisibleRepresentation.Visible = false;
            this.mCollision.Visible = false;

            mLaser = ShapeManager.AddCircle();
            mLaser.AttachTo(this, false);
            mLaser.Visible = false;

            mTongue = ShapeManager.AddAxisAlignedRectangle();
            mTongue.ScaleX = 0.5f;
            mTongue.AttachTo(this, false);
            mTongue.Visible = false;

            this.X = DEFAULT_CHAMEL_POS;
            this.XVelocity = DEFAULT_CHAMEL_VELOCITY;

            mLaser.RelativeX = DEFAULT_CHAMEL_POS;
            this.mLastLaserPos = DEFAULT_CHAMEL_POS;
            mLaserRail = 3;

            mTongue.RelativeX = TONGUE_START;
            this.mLastTonguePos = 0.0f;

            TongueRail = -1;
        }

        public virtual void Activity()
        {
            // This code should do things like set Animations, respond to input, and so on.
            bool activateLaser = (this.X - mLastLaserPos > LASER_TRIGGER_DISTANCE);
            bool activateTongue = (this.X - mLastTonguePos > TONGUE_TRIGGER_DISTANCE);

            //Console.Out.WriteLine("Activate Laser: " + activateLaser);

            this.mVisibleRepresentation.Visible = true;
            this.mCollision.Visible = true;

            this.XVelocity = DEFAULT_CHAMEL_VELOCITY + Screens.GameScreen.SpeedMod;

            if (mIsLasering)
                activateLaser = true;

            if (mIsLicking)
                activateTongue = true;

            if (activateLaser)
            {
                LaserAttack();
            }

            if (activateTongue)
            {
                TongueAttack();
            }

            if (mTongue.Visible)
                TongueRail = mTongueRail - 1;
            else
                TongueRail = -1;

            if (mTongue.Visible)
            {
                if (mTongueRail == 1)
                {
                    head3.Visible = false;
                    heado3.Visible = true;
                }
                else if (mTongueRail == 2)
                {
                    head2.Visible = false;
                    heado2.Visible = true;
                }
                else if (mTongueRail == 3)
                {
                    head1.Visible = false;
                    heado1.Visible = true;
                }
            }
        }

        public virtual void Destroy()
        {
            // Remove self from the SpriteManager:
            SpriteManager.RemovePositionedObject(this);

            // Remove any other objects you've created:
            SpriteManager.RemoveSprite(mVisibleRepresentation);
            ShapeManager.Remove(mCollision);
        }

        #region Attacks
               
        private void LaserAttack()
        {
            if (!mIsLasering)
            {
                mLaser.RelativeY = RAIL_DISTANCE;
                mLaser.RelativeX = 0.0f;
                mLaser.Visible = true;

                mLaser.RelativeXVelocity = LASER_VELOCITY;
                mIsLasering = true;
                mLaserRail = 3;
            }
            else
            {
                if ((mLaserRail == 3) && (mLaser.X >= SpriteManager.Camera.AbsoluteRightXEdgeAt(0)))
                {
                    mLaserRail = 2;
                    mLaser.RelativeY = 0.0f;
                    mLaser.RelativeXVelocity = -(LASER_VELOCITY);
                }
                else if ((mLaserRail == 2) && (mLaser.X <= this.X - 5.0f))
                {
                    mLaserRail = 1;
                    mLaser.RelativeY = -RAIL_DISTANCE;
                    mLaser.RelativeXVelocity = LASER_VELOCITY;
                    //mLaser.RelativeX = this.X - 7.0f;
                }
                else if ((mLaserRail == 1) && (mLaser.X >= SpriteManager.Camera.AbsoluteRightXEdgeAt(0)))
                {
                    mLaserRail = 0;
                    mLaser.RelativeXVelocity = 0;
                    mLaser.Visible = false;
                    mIsLasering = false;
                    mLastLaserPos = this.X + (Screens.GameScreen.SpeedMod * 20);
                }

                #region Old Laser Code
                //if (mLaserDistance == LASER_ROW_LENGTH)
                //{
                //    mLaser.RelativeY = 0.0f;
                //    mLaser.RelativeXVelocity = -(LASER_VELOCITY);
                //}
                //else if (mLaserDistance == (LASER_ROW_LENGTH * 2))
                //{
                //    mLaser.RelativeY = -5.0f;
                //    mLaser.RelativeXVelocity = LASER_VELOCITY;
                //}
                //else if (mLaserDistance == (LASER_ROW_LENGTH * 3))
                //{
                //    mLaser.RelativeXVelocity = 0;
                //    mLaser.Visible = false;
                //    mIsLasering = false;
                //    mLastLaserPos = this.X;
                //}

                //mLaserDistance++;

                #endregion
            }
        }

        private void TongueAttack()
        {
            if (!mIsLicking)
            {
                mTongue.Visible = true;

                mTongue.RelativeXVelocity = TONGUE_VELOCITY;
                mIsLicking = true;
                mTongueRail = CreateRandomNumber();

                if (mTongueRail == 3)
                {
                    mTongue.RelativeX = TONGUE_START;
                    mTongue.RelativeY = RAIL_DISTANCE;
                }
                else if (mTongueRail == 2)
                {
                    mTongue.RelativeX = TONGUE_START - 5.0f;
                    mTongue.RelativeY = 0.0f;
                }
                else if (mTongueRail == 1)
                {
                    mTongue.RelativeX = TONGUE_START - 10.0f;
                    mTongue.RelativeY = -(RAIL_DISTANCE);
                }
            }
            else
            {
                mTongue.ScaleX += 0.25f;
                if (mTongue.X >= SpriteManager.Camera.AbsoluteRightXEdgeAt(0) + 5.0f)
                {
                    mTongue.RelativeXVelocity = 0.0f;
                    mTongue.ScaleX = 0.5f;
                    mTongue.Visible = false;

                    mIsLicking = false;
                    mLastTonguePos = this.X + (Screens.GameScreen.SpeedMod * 20);
                }
            }
        }

        #endregion

        private int CreateRandomNumber()
        {
            Random r = new Random();

            return r.Next(1, 4);
        }
        
        #endregion
    }
}