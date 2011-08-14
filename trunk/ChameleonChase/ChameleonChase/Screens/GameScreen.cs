﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FlatRedBall;
using FlatRedBall.Graphics;
using FlatRedBall.Math.Geometry;
using FlatRedBall.Input;

namespace ChameleonChase.Screens
{
    public class GameScreen : Screen
    {
        CrippleMan player;
        Chameleon chameleon;

        List<Obstacle> obstaclePool;
        List<Booster> boosterPool;

        public static float SpeedMod = 0;

        int score;
        Text scoreText;

        #region Methods

        #region Constructor and Initialize

        public GameScreen() : base("GameScreen")
        {
            // Don't put initialization code here, do it in
            // the Initialize method below
            //   |   |   |   |   |   |   |   |   |   |   |
            //   |   |   |   |   |   |   |   |   |   |   |
            //   V   V   V   V   V   V   V   V   V   V   V

        }

        public override void Initialize(bool addToManagers)
        {
            // Set the screen up here instead of in the Constructor to avoid
            // exceptions occurring during the constructor.

            player = new CrippleMan(FlatRedBallServices.GlobalContentManager);

            chameleon = new Chameleon(FlatRedBallServices.GlobalContentManager);

            obstaclePool = new List<Obstacle>();

            for (int x = 0; x < 6; x++)
            {
                obstaclePool.Add(new Obstacle(FlatRedBallServices.GlobalContentManager));
            }

            boosterPool = new List<Booster>();

            for (int y = 0; y < 3; y++)
            {
                boosterPool.Add(new Booster(FlatRedBallServices.GlobalContentManager));
            }

            //Sprite referencePoint = SpriteManager.AddSprite("redball.bmp", FlatRedBallServices.GlobalContentManager);

            score = 0;

            scoreText = TextManager.AddText("" + score);
            scoreText.AttachTo(SpriteManager.Camera, false);
            scoreText.RelativeX = -20.0f;
            scoreText.RelativeY = 15.0f;
            scoreText.RelativeZ = -40.0f;
            scoreText.SetColor(0, 0, 0);
			
			// AddToManagers should be called LAST in this method:
			if(addToManagers)
			{
				AddToManagers();
			}
        }

		public override void AddToManagers()
        {
		
		
		}
		
        #endregion

        #region Public Methods

        private void PlaceObstacles()
        {
            foreach (Obstacle item in obstaclePool)
            {
                if (item.X - SpriteManager.Camera.X < -30.0f)
                {
                    RepositionObstacles(item);
                }
            }

            foreach (Booster item in boosterPool)
            {
                if (item.X - SpriteManager.Camera.X < -30.0f)
                {
                    RepositionObstacles(item);
                }
            }
        }

        private void RepositionObstacles(Booster item)
        {
            item.X = FlatRedBallServices.Random.Next(0, 20) + SpriteManager.Camera.X + 50.0f;
            item.Y = FlatRedBallServices.Random.Next(-1, 2) * 10.0f;
        }

        private void RepositionObstacles(Obstacle item)
        {
            item.X = FlatRedBallServices.Random.Next(0, 20) + SpriteManager.Camera.X + 50.0f;
            item.Y = FlatRedBallServices.Random.Next(-1, 2) * 10.0f;
        }

        private void CheckCollisions()
        {
            foreach (Obstacle item in obstaclePool)
            {
                if (item.Collision.CollideAgainst(player.Collision) && !player.IsDamaged)
                {
                    player.OnHit();
                    RepositionObstacles(item);
                }
            }

            foreach (Booster item in boosterPool)
            {
                if (item.Collision.CollideAgainst(player.Collision))
                {
                    player.OnBoost();
                    RepositionObstacles(item);
                }
            }

            if (chameleon.Collision.CollideAgainst(player.Collision))
            {
                player.Destroy();
            }

            if (chameleon.Laser.CollideAgainst(player.Collision))
            {
                player.OnHit();
            }

            if (chameleon.Tongue.CollideAgainst(player.Collision))
            {
                player.OnLicked();
            }
        }

        public override void Activity(bool firstTimeCalled)
        {
            player.Activity();
            chameleon.Activity();
            PlaceObstacles();
            CheckCollisions();

            score = (int) player.X;
            scoreText.DisplayText = "Score:  " + score;

            SpeedMod = (int) (player.X / 100.0f);

            base.Activity(firstTimeCalled);
        }

        public override void Destroy()
        {
            base.Destroy();
			
			


        }

        #endregion

		
        #endregion
    }
}
