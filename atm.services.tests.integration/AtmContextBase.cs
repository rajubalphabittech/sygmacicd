namespace atm.services.tests
{
	public abstract class AtmContextBase
	{
		public AtmContext GetContext() {
			return new AtmContext("ATM");
		}
	}
}