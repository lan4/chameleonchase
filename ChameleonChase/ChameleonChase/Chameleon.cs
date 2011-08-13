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
        private const float DEFAULT_CHAMEL_VELOCITY = 51.0f;
        private const float SLOW_CHAMEL_VELOCITY = 45.0f;
        private const float DEFAULT_CHAMEL_POS = -10.0f;
        
        private const float LASER_VELOCITY = 10.0f;
        private const float LASER_TRIGGER_DISTANCE = 100.0f;
        private const int LASER_ROW_LENGTH = 90;

        private const float TONGUE_OFFSET = 200.0f;

        #endregion

        #region Fields

        // Here you'd define things that your Entity contains, like Sprites
        // or Circles:
        private Sprite mVisibleRepresentation;
        private Circle mCollision;

        private Circle mLaser;
        private Polygon mTongue;

        private float mLastLaserPos;
        private bool mIsLasering;
        private int mLaserDistance;

        // Keep the ContentManager for easy access:
        string mContentManagerName;

        #endregion

        #region Properties


        // Here you'd define properties for things
        // you want to give other Entities and game code
        // access to, like your Collision property:
        //public Circle Collision
        //{
        //    get { return mCollision; }
        //}

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
            mVisibleRepresentation = SpriteManager.AddSprite("redball.bmp", mContentManagerName);
            mVisibleRepresentation.AttachTo(this, false);

            mCollision = ShapeManager.AddCircle();
            mCollision.AttachTo(this, false);

            mLaser = ShapeManager.AddCircle();
            mLaser.AttachTo(this, false);
            mLaser.Visible = false;

            this.X = DEFAULT_CHAMEL_POS;
            this.XVelocity = DEFAULT_CHAMEL_VELOCITY;

            mLaser.RelativeX = DEFAULT_CHAMEL_POS;
            //mLaser.RelativeXVelocity = LASER_VELOCITY;
            this.mLastLaserPos = DEFAULT_CHAMEL_POS;
        }


        public virtual void Activity()
        {
            // This code should do things like set Animations, respond to input, and so on.
            bool activateLaser = (this.X - mLastLaserPos > LASER_TRIGGER_DISTANCE);

            //Console.Out.WriteLine("Activate Laser: " + activateLaser);

            if (mIsLasering)
                activateLaser = true;

            if (activateLaser)
            {
                LaserAttack();
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
                mLaser.RelativeY = 5.0f;
                mLaser.RelativeX = 0.0f;
                mLaser.Visible = true;

                mLaser.RelativeXVelocity = LASER_VELOCITY;
                mIsLasering = true;
                mLaserDistance = 0;
            }
            else
            {
                if (mLaserDistance == LASER_ROW_LENGTH)
                {
                    mLaser.RelativeY = 0.0f;
                    mLaser.RelativeXVelocity = -(LASER_VELOCITY);
                }
                else if (mLaserDistance == (LASER_ROW_LENGTH * 2))
                {
                    mLaser.RelativeY = -5.0f;
                    mLaser.RelativeXVelocity = LASER_VELOCITY;
                }
                else if (mLaserDistance == (LASER_ROW_LENGTH * 3))
                {
                    mLaser.RelativeXVelocity = 0;
                    mLaser.Visible = false;
                    mIsLasering = false;
                    mLastLaserPos = this.X;
                }

                mLaserDistance++;
            }
        }

        private void TongueAttack()
        {

        }

        #endregion
        
        #endregion
    }
}
