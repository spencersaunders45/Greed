namespace Greed_V2.Game.Casting{
   class Rock : Actor
   {
      // summary: sets the values of the rock
      public Rock()
      {
         SetText("O");
         SetColor(GenerateColor());
      }

   }
}