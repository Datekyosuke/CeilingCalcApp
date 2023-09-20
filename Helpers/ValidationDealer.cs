using WebApiDB.Models;

namespace WebApiDB.Helpers
{
    public static class ValidationDealer
    {
        public static Tuple<bool, string> DealerValidation(Dealer dealer)
        {
            if (dealer == null) return Tuple.Create(false, "Dealer is empty");
            if (dealer.FirstName.Length > 50)
                return Tuple.Create(false, "FirstName cannot be more than 50 characters"); 
            if (dealer.Telephone < 10000000000 || dealer.Telephone > 99999999999)
            {
                return Tuple.Create(false, "Invalid telephone. Must contain 11 digits!");
            }
            if (dealer.LastName.Length > 50 || dealer.LastName.Length < 2)
                return Tuple.Create(false, "LastName cannot be more than 50 and less than 2 characters");
            if (dealer.Debts > float.MaxValue || dealer.Debts < float.MinValue)
                return Tuple.Create(false, "Wrong debts! Too big (small) number");
            if (dealer.City.Length > 50 || dealer.City.Length < 2)
                return Tuple.Create(false, "City cannot be more than 50 and less than 2 characters");
             return Tuple.Create(true, "");

        }
    }
}
