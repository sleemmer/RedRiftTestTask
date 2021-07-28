namespace GameMechanics
{
	public class CardData
	{
		public int Id;
		public string IconPath;
		public string Title;
		public string Description;
		public int Mana;
		public int Attack;
		public int Health;

		public CardData(int id, string iconPath, string title, string description, int mana, int attack, int health)
		{
			Id = id;
			IconPath = iconPath;
			Title = title;
			Description = description;
			Mana = mana;
			Attack = attack;
			Health = health;
		}

		public bool IsDead()
		{
			return Health < 1;
		}
	}
}