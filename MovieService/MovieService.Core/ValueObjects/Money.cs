namespace MovieService.Core.ValueObjects
{
    public class Money
    {
        public Money(decimal amount, string currency)
        {
            Amount = amount;
            Currency = currency;
        }

        public decimal Amount { get; private set; }
        public string Currency { get; private set; }

        public override string ToString() => $"{Amount:0.00} {Currency}";
    }
}
