using System;
namespace ConsoleApp1;

interface Attackable
{
    void takeDamage(int damage);
    bool isAlive();
}

interface Attacker
{
    void attack(Attackable target);
}

class Warrior : Attackable, Attacker
{
    public int health { get; set; }
    public int strength { get; set; }

    public void attack(Attackable target)
    {
        target.takeDamage(strength);
    }

    public void takeDamage(int damage)
    {
        health = health - damage;
    }

    public bool isAlive()
    {
        return health > 0;
    }
}

class Mage : Attackable, Attacker
{
    public int health { get; set; }
    public int mana { get; set; }
    public int spellPower { get; set; }

    public void attack(Attackable target)
    {
        if (mana >= 10)
        {
            target.takeDamage(spellPower);
            mana = mana - 10;
        }
        else
        {
            target.takeDamage(1);
        }
    }

    public void takeDamage(int damage)
    {
        health = health - damage;
    }

    public bool isAlive()
    {
        return health > 0;
    }
}

class Program3
{
    static void Main()
    {
        Warrior warrior = new Warrior();
        warrior.health = 100;
        warrior.strength = 15;

        Mage mage = new Mage();
        mage.health = 80;
        mage.mana = 50;
        mage.spellPower = 20;

        while (warrior.isAlive() && mage.isAlive())
        {
            warrior.attack(mage);
            if (mage.isAlive())
            {
                mage.attack(warrior);
            }
            Console.WriteLine("Воин: " + warrior.health + " Маг: " + mage.health);
        }

        if (warrior.isAlive())
        {
            Console.WriteLine("Воин победил");
        }
        else
        {
            Console.WriteLine("Маг победил");
        }
    }
}