public class Data : MyInterface
{
    string txt = "Paddle";
    int value = 0;

    public int Value => value;

    public virtual void Print()
    {
        Console.WriteLine($"Affichage : Name {ToString()} - Value {Value} ");
    }

    public virtual void Update()
    {
        value += 1;
        Print();
    }

    public override string ToString()
    {
        return txt;
    }
}




MyObj = new Data();