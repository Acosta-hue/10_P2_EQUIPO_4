using System.Text;

Console.OutputEncoding = Encoding.UTF8;

Console.WriteLine("=== Diagnóstico de motores ===");

string respuesta;
int contador = 0;

// Ciclo while: se repite mientras el técnico responda "s"
do
{
    contador++;
    Console.WriteLine($"\n--- Motor #{contador} ---");

    // Objeto: motor creado a partir de la clase Motor
    string id = Entrada.LeerTexto("Identificador del motor: ");
    Motor motor = new Motor(id, 4);

    // Ciclo for: captura de las cuatro mediciones de corriente
    for (int i = 0; i < motor.Mediciones.Length; i++)
    {
        motor.Mediciones[i] = Entrada.LeerDouble($"Corriente {i + 1}/4 (A): ", 0, false);
    }

    Console.WriteLine($"\nMotor {motor.Id}: promedio = {motor.Promedio():F2} A");
    Console.WriteLine($"Diagnóstico: {motor.Diagnostico()}");

    // Pregunta de continuación (solo acepta s o n)
    while (true)
    {
        Console.Write("\n¿Desea registrar otro motor? (s/n): ");
        respuesta = (Console.ReadLine() ?? "").Trim().ToLower();
        if (respuesta == "s" || respuesta == "n") break;
        Console.WriteLine("  Respuesta inválida: escriba s o n.");
    }
} while (respuesta == "s");

Console.WriteLine($"\nSe diagnosticaron {contador} motor(es). Fin del programa.");

// ===== Clase: Motor =====
class Motor
{
    // Propiedades
    public string Id { get; }             // string: identificador
    public double[] Mediciones { get; }   // double[]: corrientes en A

    public Motor(string id, int numMediciones)
    {
        Id = id;
        Mediciones = new double[numMediciones];
    }

    // Método: promedio de corriente (A), protegido contra división entre cero
    public double Promedio()
    {
        if (Mediciones.Length == 0) return 0.0;
        double suma = 0;
        foreach (double m in Mediciones) suma += m;
        return suma / Mediciones.Length;
    }

    // Método: NORMAL si promedio <= 5 A; de lo contrario REQUIERE MANTENIMIENTO
    public string Diagnostico() => Promedio() <= 5.0 ? "NORMAL" : "REQUIERE MANTENIMIENTO";
}

// ===== Clase auxiliar: lectura segura de datos =====
static class Entrada
{
    // Lee un número decimal; repite mientras el dato no sea válido o no cumpla el mínimo
    public static double LeerDouble(string mensaje, double minimo, bool estricto)
    {
        while (true)
        {
            Console.Write(mensaje);
            string? texto = Console.ReadLine();
            if (double.TryParse((texto ?? "").Trim().Replace(',', '.'),
                System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture, out double v)
                && (estricto ? v > minimo : v >= minimo))
                return v;
            Console.WriteLine(estricto ? $"  Valor inválido: debe ser un número mayor que {minimo}."
                                       : $"  Valor inválido: debe ser un número mayor o igual que {minimo}.");
        }
    }

    // Lee un entero dentro de un rango; repite mientras sea inválido
    public static int LeerEntero(string mensaje, int minimo, int maximo)
    {
        while (true)
        {
            Console.Write(mensaje);
            if (int.TryParse(Console.ReadLine()?.Trim(), out int v) && v >= minimo && v <= maximo)
                return v;
            Console.WriteLine($"  Valor inválido: debe ser un entero entre {minimo} y {maximo}.");
        }
    }

    // Lee texto no vacío
    public static string LeerTexto(string mensaje)
    {
        while (true)
        {
            Console.Write(mensaje);
            string? t = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(t)) return t;
            Console.WriteLine("  El texto no puede estar vacío.");
        }
    }
}
