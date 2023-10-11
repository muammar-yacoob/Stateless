namespace Stateless.Candy.Events
{
    public class CandyFired
    {
        public int CandyAmount = 1;

        public CandyFired(int candyAmount)
        {
            this.CandyAmount = candyAmount;
        }
    }
}