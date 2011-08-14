using System;
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
        private Sprite head0;

        private Sprite heado1;
        private Sprite heado2;
        private Sprite heado0;

        private Sprite mTongue;
        private Sprite mLaser;

        //private Circle mCollision;
        private Polygon mCollision;

        private Circle mLaserCollision;
        private AxisAlignedRectangle mTongueCollision;

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
            get { return mLaserCollision; }
        }

        public AxisAlignedRectangle Tongue
        {
            get { return mTongueCollision; }
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
            head1.RelativeY = 2.0f;

            head2 = SpriteManager.AddSprite("Cham_Closed.png", mContentManagerName);

            texturePixelWidth = head2.Texture.Width;
            texturePixelHeight = head2.Texture.Height;

            pixelsPerUnit = SpriteManager.Camera.PixelsPerUnitAt(head2.Z);

            head2.ScaleX = .5f * texturePixelWidth / pixelsPerUnit;
            head2.ScaleY = .5f * texturePixelHeight / pixelsPerUnit;

            head2.AttachTo(this, false);
            head2.RelativeX = 5.0f;
            head2.RelativeY = 12.0f;

            head0 = SpriteManager.AddSprite("Cham_Closed.png", mContentManagerName);

            texturePixelWidth = head0.Texture.Width;
            texturePixelHeight = head0.Texture.Height;

            pixelsPerUnit = SpriteManager.Camera.PixelsPerUnitAt(head0.Z);

            head0.ScaleX = .5f * texturePixelWidth / pixelsPerUnit;
            head0.ScaleY = .5f * texturePixelHeight / pixelsPerUnit;

            head0.AttachTo(this, false);
            head0.RelativeX = -5.0f;
            head0.RelativeY = -8.0f;

            heado1 = SpriteManager.AddSprite("Cham_Open.png", mContentManagerName);

            texturePixelWidth = heado1.Texture.Width;
            texturePixelHeight = heado1.Texture.Height;

            pixelsPerUnit = SpriteManager.Camera.PixelsPerUnitAt(heado1.Z);

            heado1.ScaleX = .5f * texturePixelWidth / pixelsPerUnit;
            heado1.ScaleY = .5f * texturePixelHeight / pixelsPerUnit;

            heado1.AttachTo(this, false);
            heado1.Visible = false;

            heado1.RelativeY = 2.0f;

            heado2 = SpriteManager.AddSprite("Cham_Open.png", mContentManagerName);

            texturePixelWidth = heado2.Texture.Width;
            texturePixelHeight = heado2.Texture.Height;

            pixelsPerUnit = SpriteManager.Camera.PixelsPerUnitAt(heado2.Z);

            heado2.ScaleX = .5f * texturePixelWidth / pixelsPerUnit;
            heado2.ScaleY = .5f * texturePixelHeight / pixelsPerUnit;

            heado2.AttachTo(this, false);
            heado2.RelativeX = 5.0f;
            heado2.RelativeY = 12.0f;
            heado2.Visible = false;

            heado0 = SpriteManager.AddSprite("Cham_Open.png", mContentManagerName);

            texturePixelWidth = heado0.Texture.Width;
            texturePixelHeight = heado0.Texture.Height;

            pixelsPerUnit = SpriteManager.Camera.PixelsPerUnitAt(heado0.Z);

            heado0.ScaleX = .5f * texturePixelWidth / pixelsPerUnit;
            heado0.ScaleY = .5f * texturePixelHeight / pixelsPerUnit;

            heado0.AttachTo(this, false);
            heado0.RelativeX = -5.0f;
            heado0.RelativeY = -8.0f;
            heado0.Visible = false;

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
            mVisibleRepresentation.Visible = false;
            mCollision.Visible = false;

            mLaserCollision = ShapeManager.AddCircle();
            mLaserCollision.AttachTo(this, false);
            mLaserCollision.Visible = false;

            mLaser = SpriteManager.AddSprite("SawBlade.png", mContentManagerName);

            texturePixelWidth = mLaser.Texture.Width;
            texturePixelHeight = mLaser.Texture.Height;

            pixelsPerUnit = SpriteManager.Camera.PixelsPerUnitAt(mLaser.Z);

            mLaser.ScaleX = .25f * texturePixelWidth / pixelsPerUnit;
            mLaser.ScaleY = .25f * texturePixelHeight / pixelsPerUnit;

            mLaser.AttachTo(mLaserCollision, false);
            mLaser.Visible = true;

            mTongueCollision = ShapeManager.AddAxisAlignedRectangle();
            mTongueCollision.ScaleX = 0.5f;
            mTongueCollision.AttachTo(this, false);
            mTongueCollision.Visible = false;

            mTongue = SpriteManager.AddSprite("Tongue.png", mContentManagerName);
            mTongue.ScaleX = mTongueCollision.ScaleX;
            mTongue.AttachTo(this, false);
            mTongue.Visible = false;

            this.X = DEFAULT_CHAMEL_POS;
            this.XVelocity = DEFAULT_CHAMEL_VELOCITY;

            mLaserCollision.RelativeX = DEFAULT_CHAMEL_POS;
            this.mLastLaserPos = DEFAULT_CHAMEL_POS;
            mLaserRail = 3;

            mTongueCollision.RelativeX = TONGUE_START;
            this.mLastTonguePos = 0.0f;

            TongueRail = -1;
        }

        public virtual void Activity()
        {
            // This code should do things like set Animations, respond to input, and so on.
            bool activateLaser = (this.X - mLastLaserPos > LASER_TRIGGER_DISTANCE);
            bool activateTongue = (this.X - mLastTonguePos > TONGUE_TRIGGER_DISTANCE);

            //Console.Out.WriteLine("Activate Laser: " + activateLaser);

            mVisibleRepresentation.Visible = true;

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

            if (mTongueCollision.Visible)
                TongueRail = mTongueRail;
            else
                TongueRail = -1;

            //if (mTongueCollision.Visible)
            //{
            //    if (mTongueRail == 1)
            //    {
            //        head0.Visible = false;
            //        heado0.Visible = true;
            //    }
            //    else if (mTongueRail == 2)
            //    {
            //        head2.Visible = false;
            //        heado2.Visible = true;
            //    }
            //    else if (mTongueRail == 3)
            //    {
            //        head1.Visible = false;
            //        heado1.Visible = true;
            //    }
            //}
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
                mLaserCollision.RelativeY = RAIL_DISTANCE;
                mLaserCollision.RelativeX = 0.0f;
                mLaser.Visible = true;

                mLaserCollision.RelativeXVelocity = LASER_VELOCITY;
                mIsLasering = true;
                mLaserRail = 2;
            }
            else
            {
                if ((mLaserRail == 2) && (mLaserCollision.X >= SpriteManager.Camera.AbsoluteRightXEdgeAt(0)))
                {
                    mLaserRail = 1;
                    mLaserCollision.RelativeY = 0.0f;
                    mLaserCollision.RelativeXVelocity = -(LASER_VELOCITY);
                }
                else if ((mLaserRail == 1) && (mLaserCollision.X <= this.X - 5.0f))
                {
                    mLaserRail = 0;
                    mLaserCollision.RelativeY = -RAIL_DISTANCE;
                    mLaserCollision.RelativeXVelocity = LASER_VELOCITY;
                    //mLaserCollision.RelativeX = this.X - 7.0f;
                }
                else if ((mLaserRail == 0) && (mLaserCollision.X >= SpriteManager.Camera.AbsoluteRightXEdgeAt(0)))
                {
                    mLaserRail = -1;
                    mLaserCollision.RelativeXVelocity = 0;
                    mLaser.Visible = false;
                    mIsLasering = false;
                    mLastLaserPos = this.X + (Screens.GameScreen.SpeedMod * 20);
                }

                #region Old Laser Code
                //if (mLaserDistance == LASER_ROW_LENGTH)
                //{
                //    mLaserCollision.RelativeY = 0.0f;
                //    mLaserCollision.RelativeXVelocity = -(LASER_VELOCITY);
                //}
                //else if (mLaserDistance == (LASER_ROW_LENGTH * 2))
                //{
                //    mLaserCollision.RelativeY = -5.0f;
                //    mLaserCollision.RelativeXVelocity = LASER_VELOCITY;
                //}
                //else if (mLaserDistance == (LASER_ROW_LENGTH * 3))
                //{
                //    mLaserCollision.RelativeXVelocity = 0;
                //    mLaserCollision.Visible = false;
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

                mTongueCollision.RelativeXVelocity = TONGUE_VELOCITY;
                mTongue.RelativeXVelocity = TONGUE_VELOCITY;

                mIsLicking = true;
                mTongueRail = CreateRandomNumber();

                if (mTongueRail == 2)
                {
                    mTongueCollision.RelativeX = TONGUE_START;
                    mTongueCollision.RelativeY = RAIL_DISTANCE;
                    mTongue.RelativeX = mTongueCollision.RelativeX;
                    mTongue.RelativeY = mTongueCollision.RelativeY;
                }
                else if (mTongueRail == 1)
                {
                    mTongueCollision.RelativeX = TONGUE_START - 5.0f;
                    mTongueCollision.RelativeY = 0.0f;
                    mTongue.RelativeX = mTongueCollision.RelativeX;
                    mTongue.RelativeY = mTongueCollision.RelativeY;
                }
                else if (mTongueRail == 0)
                {
                    mTongueCollision.RelativeX = TONGUE_START - 10.0f;
                    mTongueCollision.RelativeY = -(RAIL_DISTANCE);
                    mTongue.RelativeX = mTongueCollision.RelativeX;
                    mTongue.RelativeY = mTongueCollision.RelativeY;
                }

                if (mTongueRail == 0)
                {
                    head0.Visible = false;
                    heado0.Visible = true;
                }
                else if (mTongueRail == 1)
                {
                    head1.Visible = false;
                    heado1.Visible = true;
                }
                else if (mTongueRail == 2)
                {
                    head2.Visible = false;
                    heado2.Visible = true;
                }
            }
            else
            {
                mTongueCollision.ScaleX += 0.25f;
                mTongue.ScaleX += 0.25f;

                if (mTongueCollision.X >= SpriteManager.Camera.AbsoluteRightXEdgeAt(0) + 5.0f)
                {
                    mTongueCollision.RelativeXVelocity = 0.0f;
                    mTongueCollision.ScaleX = 0.5f;
                    mTongue.Visible = false;

                    mIsLicking = false;
                    mLastTonguePos = this.X + (Screens.GameScreen.SpeedMod * 20);

                    if (mTongueRail == 0)
                    {
                        head0.Visible = true;
                        heado0.Visible = false;
                    }
                    else if (mTongueRail == 1)
                    {
                        head1.Visible = true;
                        heado1.Visible = false;
                    }
                    else if (mTongueRail == 2)
                    {
                        head2.Visible = true;
                        heado2.Visible = false;
                    }
                }
            }
        }

        #endregion

        private int CreateRandomNumber()
        {
            Random r = new Random();

            return r.Next(0, 3);
        }

        #endregion
    }
}
