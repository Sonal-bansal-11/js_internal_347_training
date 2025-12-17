using System;

namespace PatientManagementSystem
{
    // DELEGATE
    public delegate double BillingStrategy(double amount);

    // BASE CLASS
    public abstract class Patient
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }

        public Patient(string name, int id, int age, string gender)
        {
            this.Name = name;
            this.Id = id;
            this.Age = age;
            this.Gender = gender;
        }

        public abstract string GetPatientType();
        public abstract double GetBaseCost();
    }

    // TYPE 1: Resident
    public class ResidentPatient : Patient
    {
        public ResidentPatient(string name, int id, int age, string gender) 
            : base(name, id, age, gender) { }

        public override string GetPatientType()
        {
            return "Resident (General Ward)";
        }

        public override double GetBaseCost()
        {
            return 2000.00;
        }
    }

    // TYPE 2: Emergency
    public class EmergencyPatient : Patient
    {
        public EmergencyPatient(string name, int id, int age, string gender) 
            : base(name, id, age, gender) { }

        public override string GetPatientType()
        {
            return "Emergency (ICU)";
        }

        public override double GetBaseCost()
        {
            return 10000.00;
        }
    }

    // TYPE 3: Outpatient
    public class Outpatient : Patient
    {
        public Outpatient(string name, int id, int age, string gender) 
            : base(name, id, age, gender) { }

        public override string GetPatientType()
        {
            return "Outpatient (Checkup)";
        }

        public override double GetBaseCost()
        {
            return 500.00;
        }
    }

    // PUBLISHER
    public class HospitalManager
    {
        public event EventHandler<string> OnHospitalEvent;

        public void AdmitPatient(Patient p)
        {
            // Trigger Event
            if (OnHospitalEvent != null)
            {
                // Replaced $"" with String.Format()
                string msg = string.Format("ADMISSION: {0} ({1}, {2}yrs) assigned to {3}", 
                    p.Name, p.Gender, p.Age, p.GetPatientType());
                
                OnHospitalEvent(this, msg);
            }
        }

        public void GenerateBill(Patient p, BillingStrategy strategy)
        {
            double baseAmount = p.GetBaseCost();
            double finalBill = strategy(baseAmount);

            Console.WriteLine("");
            Console.WriteLine("   [Details] Patient Type: " + p.GetPatientType());
            Console.WriteLine("   [Details] Standard Rate: " + baseAmount);
            Console.WriteLine("   [Details] Final Bill Amount: " + finalBill);

            if (OnHospitalEvent != null)
            {
                // Replaced $"" with String.Format()
                string msg = string.Format("BILLING: Invoice generated for {0}. Final Amount: {1}", 
                    p.Name, finalBill);
                
                OnHospitalEvent(this, msg);
            }
        }
    }

    // MAIN PROGRAM
    class Program
    {
        // Define static methods for billing logic so we don't need lambdas if they fail too
        static double TaxLogic(double amt)
        {
            return amt + (amt * 0.10);
        }

        static double InsuranceLogic(double amt)
        {
            return amt * 0.50;
        }

        static void Main(string[] args)
        {
            HospitalManager hospital = new HospitalManager();

            // SUBSCRIBE
            hospital.OnHospitalEvent += new EventHandler<string>(OnNotification);

            while (true)
            {
                Console.WriteLine("\n============================================");
                Console.WriteLine("   SMART HOSPITAL SYSTEM");
                Console.WriteLine("============================================");

                Console.Write("Enter Patient Name: ");
                string name = Console.ReadLine();
                
                Console.Write("Enter Age: ");
                string ageInput = Console.ReadLine();
                int age = int.Parse(ageInput);

                Console.Write("Enter Gender (M/F): ");
                string gender = Console.ReadLine();

                Console.WriteLine("\nSelect Patient Condition:");
                Console.WriteLine("1. Regular Illness (General Ward)");
                Console.WriteLine("2. Critical Condition (ICU)");
                Console.WriteLine("3. Minor Checkup (OPD)");
                Console.Write("Choice: ");
                string typeChoice = Console.ReadLine();

                int id = 101; // Simple static ID to avoid Random errors

                Patient patient = null;

                if (typeChoice == "1")
                    patient = new ResidentPatient(name, id, age, gender);
                else if (typeChoice == "2")
                    patient = new EmergencyPatient(name, id, age, gender);
                else
                    patient = new Outpatient(name, id, age, gender);

                hospital.AdmitPatient(patient);

                Console.WriteLine("\nSelect Payment Method:");
                Console.WriteLine("1. Cash (Standard + 10% Tax)");
                Console.WriteLine("2. Insurance (50% Covered)");
                Console.Write("Choice: ");
                string billChoice = Console.ReadLine();

                BillingStrategy selectedStrategy = null;

                if (billChoice == "1")
                {
                    selectedStrategy = new BillingStrategy(TaxLogic);
                    Console.WriteLine("-> Applied: Cash Tax Logic");
                }
                else
                {
                    selectedStrategy = new BillingStrategy(InsuranceLogic);
                    Console.WriteLine("-> Applied: Insurance Logic");
                }

                hospital.GenerateBill(patient, selectedStrategy);

                Console.WriteLine("\nPress Enter for next patient...");
                Console.ReadLine();
                Console.Clear();
            }
        }

        // Event Handler Method
        static void OnNotification(object sender, string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(" >>> SYSTEM ALERT: " + message);
            Console.ResetColor();
        }
    }
}