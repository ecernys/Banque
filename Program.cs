using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banque
{
    class Program
    {
        static void Main(string[] args)
        {
            CultureInfo.CurrentCulture = new CultureInfo("en-US", false);
            #region Récupération du chemin des fichiers

            string path = Directory.GetCurrentDirectory();
            // Fichiers entrée
            //string acctPath = string.Empty;
            string acctPath = path + @"\Comptes_.txt";
            //string trxnPath = string.Empty;
            string trxnPath = path + @"\Transactions_.txt";
            // Fichiers sortie
            string sttsTrxnPath = path + @"\StatutOpe_.txt";

            if (args.Length > 0)
            {
                foreach (var arg in args)
                {
#if DEBUG
                    Console.WriteLine(arg);
#endif
                    if (File.Exists(arg))
                    {
                        if (arg.Contains("Comptes_"))
                            acctPath = arg;
                        else if (arg.Contains("Transactions_"))
                            trxnPath = arg;
                    }
                }
            }
            #endregion
            // La suite (votre code) ici
            Utils utils = new Utils();
            utils.readAccounts(acctPath);
            utils.readTransactions(trxnPath);
            utils.writeTransactionsStatus(sttsTrxnPath);

            // Keep the console window open
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
