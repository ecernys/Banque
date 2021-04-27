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
            string mngrPath = path + @"\Gestionnaires_3.txt";
            string acctPath = path + @"\Comptes_3.txt";
            string trxnPath = path + @"\Transactions_3.txt";
            // Fichiers sortie
            string sttsAcctPath = path + @"\StatutOpe_3.txt"; 
            string sttsTrxnPath = path + @"\StatutTra_3.txt";
            string mtrlPath = path + @"\Metrologie_3.txt";

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
            utils.readClients(mngrPath);
            utils.readAccounts(acctPath);
            utils.readTransactions(trxnPath);
            utils.ProcessClients();
            utils.ProcessOperations();
            utils.writeTransactionsStatus(sttsTrxnPath);
            utils.writeOperationsStatus(sttsAcctPath);
            utils.writeStatistics(mtrlPath);

            // Keep the console window open
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
