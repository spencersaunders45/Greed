using System.Collections.Generic;
using Greed_V2.Game.Casting;
using Greed_V2.Game.Services;


namespace Greed_V2.Game.Scripting
{
   /// <summary>
   /// <para>An output action that draws all the actors.</para>
   /// <para>The responsibility of DrawActorsAction is to draw each of the actors.</para>
   /// </summary>
   public class DrawActorsAction : Actions
   {
      private VideoService videoService;

      /// <summary>
      /// Constructs a new instance of ControlActorsAction using the given KeyboardService.
      /// </summary>
      public DrawActorsAction(VideoService videoService)
      {
         this.videoService = videoService;
      }

      /// <inheritdoc/>
      public void Execute(Cast cast, Script script)
      {
         Actor score = cast.GetFirstActor("score");
         // List<Actor> messages = cast.GetActors("messages");
         
         videoService.ClearBuffer();
         videoService.DrawActor(score);
         // videoService.DrawActors(messages);
         videoService.FlushBuffer();
      }
   }
}