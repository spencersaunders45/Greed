using System.Collections.Generic;
using Greed_V2.Game.Casting;
using Greed_V2.Game.Services;


namespace Greed_V2.Game.Directing
{
   /// <summary>
   /// <para>A person who directs the game.</para>
   /// <para>
   /// The responsibility of a Director is to control the sequence of play.
   /// </para>
   /// </summary>
   public class Director
   {
      private KeyboardService keyboardService = null;
      private VideoService videoService = null;
      private Score score = null;
      private Random random = new Random();

      /// <summary>
      /// Constructs a new instance of Director using the given KeyboardService and VideoService.
      /// </summary>
      /// <param name="keyboardService">The given KeyboardService.</param>
      /// <param name="videoService">The given VideoService.</param>
      public Director(KeyboardService keyboardService, VideoService videoService)
      {
         this.keyboardService = keyboardService;
         this.videoService = videoService;
         score = new Score();
      }

      /// <summary>
      /// Starts the game by running the main game loop for the given cast.
      /// </summary>
      /// <param name="cast">The given cast.</param>
      public void StartGame(Cast cast, int CELL_SIZE, int maxX, int maxY)
      {
         videoService.OpenWindow();
         while (videoService.IsWindowOpen())
         {
               GetInputs(cast);
               AddMenerals(cast, maxX);
               MineralUpdate(cast, CELL_SIZE, maxX, maxY);
               DoUpdates(cast);
               DoOutputs(cast);
               RemoveMinerals(cast, maxY);
         }
         videoService.CloseWindow();
      }

      /// <summary>
      /// Gets directional input from the keyboard and applies it to the robot.
      /// </summary>
      /// <param name="cast">The given cast.</param>
      private void GetInputs(Cast cast)
      {
         Actor robot = cast.GetFirstActor("robot");
         Point velocity = keyboardService.GetDirection();
         robot.SetVelocity(velocity);     
      }

      //Summary: Adds new mernerals to the screen
      private void AddMenerals(Cast cast, int maxX)
      {
         //Generates a number of new rocks/gems to be created
         int numOfRocks = random.Next(0, 2);
         int numOfGems = random.Next(0, 2);

         //Adds the new gems to the cast
         for (int i = 0; i < numOfGems; i++)
         {
            Gem gem = new Gem();
            gem.SetFontSize(15);
            gem.SetPosition(new Point(random.Next(0,maxX),0));
            cast.AddActor("gem", gem);
         }

         //Adds the new rocks to the cast
         for (int i = 0; i < numOfGems; i++)
         {
            Rock rock = new Rock();
            rock.SetFontSize(15);
            rock.SetPosition(new Point(random.Next(0,maxX),0));
            cast.AddActor("rock", rock);
         }

      }

      // summary: Updates the minerals positions
      private void MineralUpdate(Cast cast, int CELL_SIZE, int maxX, int maxY)
      {
         // Moves the rocks down
         foreach (Rock i in cast.GetActors("rock"))
         {
            Point direction = new Point(0, 1);
            direction = direction.Scale(CELL_SIZE);

            i.SetVelocity(direction);
            i.MoveNext(maxX, maxY);
         }

         //Moves the gems down
         foreach (Gem i in cast.GetActors("gem"))
         {
            Point direction = new Point(0, 1);
            direction = direction.Scale(CELL_SIZE);

            i.SetVelocity(direction);
            i.MoveNext(maxX, maxY);
         }

      }

      // Summary: Removes minerals that have made it to the bottom of the screen
      private void RemoveMinerals(Cast cast, int maxY)
      {
         // Removes the rock when it gets to the bottom of the screen
         foreach (Rock i in cast.GetActors("rock"))
         {
            int y = i.GetPosition().GetY();
            if (y == maxY - i.GetFontSize())
            {
               cast.RemoveActor("rock", i);
            }
         }

         // Removes the gem when it gets to the bottom of the screen
         foreach (Gem i in cast.GetActors("gem"))
         {
            int y = i.GetPosition().GetY();
            if (y == maxY - i.GetFontSize())
            {
               cast.RemoveActor("gem", i);
            }
         }
      }

      /// <summary>
      /// Updates the robot's position and resolves any collisions with artifacts.
      /// </summary>
      /// <param name="cast">The given cast.</param>
      private void DoUpdates(Cast cast)
      {
         Actor banner = cast.GetFirstActor("banner");
         Actor robot = cast.GetFirstActor("robot");
         List<Actor> rocks = cast.GetActors("rock");
         List<Actor> gems = cast.GetActors("gem");

         banner.SetText("");
         int maxX = videoService.GetWidth();
         int maxY = videoService.GetHeight();
         robot.MoveNext(maxX, maxY);

         foreach (Actor actor in rocks)
         {
            if (robot.GetPosition().Equals(actor.GetPosition()))
            {
               score.AddPoints(-1);
            }
         } 

         foreach (Gem gem in gems)
         {
            if (robot.GetPosition().Equals(gem.GetPosition()))
            {
               score.AddPoints(1);
            }
         } 
      }

      /// <summary>
      /// Draws the actors on the screen.
      /// </summary>
      /// <param name="cast">The given cast.</param>
      public void DoOutputs(Cast cast)
      {
         List<Actor> actors = cast.GetAllActors();
         videoService.ClearBuffer();
         videoService.DrawActor(score);
         videoService.DrawActors(actors);
         videoService.FlushBuffer();
      }

   }
}