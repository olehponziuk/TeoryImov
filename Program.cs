//VAR - 3, Ponziuk Oleh,
//312 - B
/*Лізингова компанія закуповує обладнання промислового призначення
   з метою здачі його в оренду. При цьому можливі значні збитки із-за недостатньо добре дослідженого ринку. Виникають чотири можливі варіанти дій
   в залежності від формування попиту на обладнання. Збитки при цьому
   складають відповідно 300, 100, 200, та 400 грош.од. Відомі ймовірності цих
   подій: р1=0,2, р2=0,3, р3= 0,1 та р4= 0,4. Знайти величину сподіваних збитків
   та інші показники ризикованості.
 */

double[] arrP = new[] { 0.2, 0.3, 0.1, 0.4 };
double[] arrR = new[] { -300.0, -100.0, -200.0, -400.0 };
double Z = -220.0;

Console.WriteLine($"У якості показника кількісної оцінки ефективності обчислимо" +
                  $"\nсподівані значення збитків M = {M(arrP, arrR)}");
Console.WriteLine($"Велечина сподіаних збитків K = {K_costs(Z, arrP, arrR)}");
Console.WriteLine($"Оцінимо ризикованість покупки обладнання, використовуючи різні показники" +
                  $"\nкількісної оцінки ризику. Обчислимо у якості показника кількісної оцінки " +
                  $"\nризику дисперсію (варіацію): V = {V(arrP, arrR)}" +
                  $"\nта середньоквадратичне відхилення: sigma = {sigma(arrP, arrR)}");
Console.WriteLine($"Обчислимо як показник кількісної " +
                  $"\nоцінки ризику коефіцієнт варіації: CV = {CV(arrP, arrR)}");
Console.WriteLine($"Доречно також розглянути семіквадратичне відхилення (що враховує\n" +
                  $"лише несприятливі відхилення) як ще один показник" +
                  $" кількісної оцінки ризику: SSV = {SSV(arrP, arrR, Z)}" +
                  $"\nта коефіцієнт семіваріації: CSV = {CSV(arrP, arrR, Z)}");
Console.WriteLine($"Розглянемо ще такі широко розповсюджені показники кількісної оцінки\n" +
                  $"ризику як імовірність небажаної події (імовірність збитків) та величину" +
                  $"\nсподіваних збитків.Ми будемо вважати, що небажана подія полягає" +
                  $"\n в R > {Z}, і обчислимо для кожного з товірів p = {p_forBadEvent(arrR, arrP, Z)}" +
                  $"\nУ величині сподіваних збитків будемо враховувати і випадок " +
                  $"\n,що вказує на ризик невикористаних можливостей. R > {Z} : Z_a = {z_forBadEvent(arrR, arrP, Z)}");



double M_plus(double Z, double[] P_n, double[] R_n)
{
    if (P_n.Length != R_n.Length)
        throw new LengthException("ERROR LEN");
    int n = P_n.Length;
    int[] beta_n = new int[n];

    for (int i = 0; i < n; ++i) {
        if (R_n[i] > Z)
            beta_n[i] = 1;
        else
            beta_n[i] = 0;
    }

    double SBP = 0, SBPR = 0, tmp;
    for (int i = 0; i < n; ++i)
    {
        tmp = beta_n[i] * P_n[i];
        SBP += tmp;
        SBPR += tmp * R_n[i];
    }
    
    return 1.0/(SBP * SBPR - Z);
}

double M_minus(double Z, double[] P_n, double[] R_n)
{
    if (P_n.Length != R_n.Length)
        throw new LengthException("ERROR LEN");
    int n = P_n.Length;
    int[] alpha_n = new int[n];

    for (int i = 0; i < n; ++i) {
        if (R_n[i] > Z)
            alpha_n[i] = 0;
        else
            alpha_n[i] = 1;
    }

    double SBP = 0, SBPR = 0, tmp;
    for (int i = 0; i < n; ++i)
    {
        tmp = alpha_n[i] * P_n[i];
        SBP += tmp;
        SBPR += tmp * R_n[i];
    }
    
    return 1.0/(SBP * SBPR - Z);
}

double K_costs(double Z, double[] P_n, double[] R_n)
{
    double Mplus = M_plus(Z, P_n, R_n);
    double Mminus = M_minus(Z, P_n, R_n);

    return Math.Abs(Mminus) / (Math.Abs(Mplus) + Math.Abs(Mminus));
}

double M(double[] R, double[] P)
{
    if (R.Length != P.Length)
        throw new LengthException("LEN IN M");
    int n = R.Length;
    double Sum = 0;
    for (int i = 0; i < n; i++) {
        Sum += P[i] * R[i];
    }

    return Sum;
}

double V(double[] R, double[] P)
{
    if (R.Length != P.Length)
        throw new LengthException("LEN IN V");
    int n = R.Length;
    double Sum = 0;
    double M1 = M(R, P);
    for (int i = 0; i < n; i++) {
        Sum += (R[i] - M1)*(R[i] - M1) * P[i];
    }

    return Sum;
}

double sigma(double[] R, double[] P)
{
    if (R.Length != P.Length)
        throw new LengthException("LEN IN sigma");
    return Math.Sqrt(V(R, P));
}

double CV(double[] R, double[] P)
{
    return sigma(R, P) / M(R, P);
}

double SSV(double[] R, double[] P, double Z)
{
    if (R.Length != P.Length)
        throw new LengthException("LEN IN SSV");
    int n = R.Length;
    int[] alphas = new int[n];
    double SV = 0;
    double m = M(R, P);
    for (int i = 0; i < n; i++) 
        SV += (double)(R[i] > Z? 1 : 0) * P[i] * (R[i] - m) * (R[i] - m);
    
    return Math.Sqrt(SV);
}

double CSV(double[] R, double[] P, double Z) => SSV(R, P, Z) / M(R, P);

double p_forBadEvent(double[] R, double[] P, double Z)
{
    if (!((P.Sum() - 1.0) < 0.00004))
        throw new PException("IN p_forBadEvent P.Sum != 1");
    if (R.Length != P.Length)
        throw new LengthException("LEN IN sigma");
    
    double sum = 0;
    int n = P.Length;
    for (int i = 0; i < n; i++)
        if (R[i] < Z)
            sum += P[i];
    
    return sum;
}

double z_forBadEvent(double[] R, double[] P, double Z)
{
    if (!((P.Sum() - 1.0) < 0.00004))
        throw new PException("IN p_forBadEvent P.Sum != 1");
    if (R.Length != P.Length)
        throw new LengthException("LEN IN sigma");
    
    double sum = 0;
    int n = P.Length;
    for (int i = 0; i < n; i++)
        if (R[i] < Z)
            sum += R[i] * P[i];
    
    return sum;
}

public class LengthException : System.Exception
{
    public LengthException() { }
    public LengthException(string message) : base(message) { }
}

public class PException : System.Exception
{
    public PException() { }
    public PException(string message) : base(message) { }
}