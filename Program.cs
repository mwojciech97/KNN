#region Vairables
var file = File.ReadAllLines("diabetes.csv").Skip(1);
int z0 = 0, o1 = 0;
int k = 0;
string patientData; 
List<Points> points = new List<Points>();
Points patient = new Points();
#endregion

CSVToList();
file = null;

GetPatientData();
patientData = null;
GetKNN();

#region CountDistanceAndCheckResults
CalculateDistance();
//liczymy tutaj K ilość osób, które mają i nie mają cukrzycy
for(int i = 0; i < k; i++)
{
    if (points[i].outcome.Equals(0)) z0++;
    else o1++;
}
#endregion

#region ShowResults
if (z0 > o1) Console.WriteLine("Patient does not have diabietes");
else if (o1 > z0) Console.WriteLine("Patient have diabietes");
else Console.WriteLine("It cannot be told if patient has diabetes or not"); 
Console.ReadLine();
Console.ReadKey();
#endregion

#region VoidFunctions
#region CSVToList
void CSVToList()
{
    try
    {
        foreach (string s in file)
        {
            var line = s.Split(',').ToList();
            points.Add(new Points(
                  int.Parse(line[0]),
                  int.Parse(line[1]),
                  int.Parse(line[2]),
                  int.Parse(line[3]),
                  int.Parse(line[4]),
                  float.Parse(line[5].Replace('.', ',')),
                  float.Parse(line[6].Replace('.', ',')),
                  int.Parse(line[7]),
                  int.Parse(line[8])
                  ));
        }
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
        Console.ReadKey();
    }
}
#endregion
#region CalculateDistance
/// <summary>
/// Liczymy odległość pomiędzy pacjentem, a każdą inną osobą w naszej bazie
/// </summary>
void CalculateDistance()
{
    foreach (Points p in points)
    {
        int x1 = p.pregnancies - patient.pregnancies;
        int x2 = p.age - patient.age;
        int x3 = p.bloodPressure - patient.bloodPressure;
        int x4 = p.skinThickness - patient.skinThickness;
        int x5 = p.insulin - patient.insulin;
        int x6 = p.glucose - patient.glucose;
        float x7 = p.bmi - patient.bmi;
        float x8 = p.diabetesPedigreeFunction - patient.diabetesPedigreeFunction;
        float distance = x1 * x1 + x2 * x2 + x3 * x3 + x4 * x4 + x5 * x5 + x6 * x6 + x7 * x7 + x8 * x8;
        p.score = MathF.Sqrt(distance);
    }
    //sortujemy listę według odległości od naszego pacjenta (od najmmniejszej do największej)
    points.Sort((x1, x2) => x1.score.CompareTo(x2.score));
}
#endregion
#region GetPatientData
void GetPatientData()
{
    do
    {
        Console.Clear();
        Console.WriteLine("Write patient data seperated by space in correct order (pregnancies(int), glucose(int), bloodPressure(int), " +
            "skinThickness(int), insulin(int), bmi(float), diabetesPedigreeFunction(float), age(int)).");
        patientData = Console.ReadLine();
        var check = patientData.Split(' ').ToList(); 
        if (check.Count() != 8) //Jest dokładnie 8 różnych czynników na podstawie, których sprawdzamy czy ktoś ma cukrzycę czy nie. Stąd to 8
        {
            patientData = null;
            Console.WriteLine("You have missed some patient data. Check it one more time.");
            Console.ReadKey();
        }
        else
        {
            try //Jeśli będzie problem z parsowanie stringa na int lub float, to dostaniemy informacje, że coś zostało źle podane
            {
                patient.pregnancies = int.Parse(check[0]);
                patient.glucose = int.Parse(check[1]);
                patient.bloodPressure = int.Parse(check[2]);
                patient.skinThickness = int.Parse(check[3]);
                patient.insulin = int.Parse(check[4]);
                patient.bmi = float.Parse(check[5]);
                patient.diabetesPedigreeFunction = float.Parse(check[6]);
                patient.age = int.Parse(check[7]);
            }
            catch (Exception e)
            {
                patientData = null;
                Console.WriteLine(e.Message);
                Console.ReadKey();
            }

        }
    } while (patientData == null);
}
#endregion
#region GetKNN
void GetKNN()
{
    do
    {
        Console.WriteLine("Number of K neighbours (cannot exceed " + points.Count() + "): ");
        patientData = Console.ReadLine();
        try
        {
            k = int.Parse(patientData);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            patientData = null;
            Console.ReadKey();
            Console.Clear();
        }
        if (k == 0 || k > points.Count())
        {
            patientData = null;
            Console.WriteLine("Number of K neighbours exceeded " + points.Count() + "or is equall 0. Input proper number!");
            Console.ReadKey();
            Console.Clear();
        }
    } while (patientData == null);
}
#endregion
#endregion