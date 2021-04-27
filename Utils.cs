using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banque
{
    class Utils
    {
        // Raw data
        List<RawAccount> rawAccounts = new List<RawAccount>();
        List<RawTransaction> rawTransactions = new List<RawTransaction>();
        List<RawClient> rawClients = new List<RawClient>();

        //Objects 
        Dictionary<int, Client> clients = new Dictionary<int, Client>();
        Dictionary<int, Account> accounts = new Dictionary<int, Account>();
        Dictionary<int, Transaction> transactions = new Dictionary<int, Transaction>();

        Dictionary<DateTime, object> operations = new Dictionary<DateTime, object>();

        //Statistics
        int validTransactionCount = 0;
        double validTransactionsSum = 0;
        Dictionary<int, double> fees = new Dictionary<int, double>();


        public void readClients(string clntPath)
        {
            using (StreamReader sr = new StreamReader(clntPath))
            {
                while (!sr.EndOfStream)
                {
                    string[] data = sr.ReadLine().Split(';');
                    string id = string.Empty;
                    string type = string.Empty;
                    string transactionCount = string.Empty;

                    switch (data.Length)
                    {
                        case 0:
                            break;
                        case 1:
                            id = data[0].Trim();
                            break;
                        case 2:
                            id = data[0].Trim();
                            type = data[1].Trim();
                            break;
                        default:
                            id = data[0].Trim();
                            type = data[1].Trim();
                            transactionCount = data[2].Trim();
                            break;
                    }
                    rawClients.Add(new RawClient(id, type, transactionCount));
                }
            }
        }

        /// <summary>
        /// Method to write Status of Transactions to file
        /// </summary>
        /// <param name="sttsTrxnPath">
        /// Path of output Status of transaction file
        /// </param>
        public void writeTransactionsStatus(string sttsTrxnPath)
        {
            using (StreamWriter sw = new StreamWriter(sttsTrxnPath))
            {
                foreach (var rawTransaction in rawTransactions)
                {
                    sw.WriteLine($"{rawTransaction.Id};{rawTransaction.Status}");
                }
            };
        }

        /// <summary>
        /// Method to read Accounts from file
        /// </summary>
        /// <param name="acctPath">
        /// Path of Accounts file 
        /// </param>
        public void readAccounts(string acctPath)
        {
            using (StreamReader sr = new StreamReader(acctPath))
            {
                while (!sr.EndOfStream)
                {
                    string[] data = sr.ReadLine().Split(';');
                    string id = string.Empty;
                    string date = string.Empty;
                    string balance = string.Empty;
                    string entry = string.Empty;
                    string exit = string.Empty;

                    switch (data.Length)
                    {
                        case 0:
                            break;
                        case 1:
                            id = data[0].Trim();
                            break;
                        case 2:
                            id = data[0].Trim();
                            date = data[1].Trim();
                            break;
                        case 3:
                            id = data[0].Trim();
                            date = data[1].Trim();
                            balance = data[2].Trim();
                            break;
                        case 4:
                            id = data[0].Trim();
                            date = data[1].Trim();
                            balance = data[2].Trim();
                            entry = data[3].Trim();
                            break;
                        default:
                            id = data[0].Trim();
                            date = data[1].Trim();
                            balance = data[2].Trim();
                            entry = data[3].Trim();
                            exit = data[4].Trim();
                            break;
                    }
                    rawAccounts.Add(new RawAccount(id, date, balance, entry, exit));
                }
            }
        }

        /// <summary>
        /// Method to read Transactions from file 
        /// </summary>
        /// <param name="trxnPath">
        /// Path of Transactions file 
        /// </param>
        public void readTransactions(string trxnPath)
        {
            using (StreamReader sr = new StreamReader(trxnPath))
            {
                while (!sr.EndOfStream)
                {
                    string rawData = sr.ReadLine();
                    string[] data = rawData.Split(';');
                    string id = string.Empty;
                    string date = string.Empty;
                    string amount = string.Empty;
                    string transmitter = string.Empty;
                    string receiver = string.Empty;

                    switch (data.Length)
                    {
                        case 0:
                            break;
                        case 1:
                            id = data[0].Trim();
                            break;
                        case 2:
                            id = data[0].Trim();
                            date = data[1].Trim();
                            break;
                        case 3:
                            id = data[0].Trim();
                            date = data[1].Trim();
                            amount = data[2].Trim();
                            break;
                        case 4:
                            id = data[0].Trim();
                            date = data[1].Trim();
                            amount = data[2].Trim();
                            transmitter = data[3].Trim();
                            break;
                        default:
                            id = data[0].Trim();
                            date = data[1].Trim();
                            amount = data[2].Trim();
                            transmitter = data[3].Trim();
                            receiver = data[4].Trim();
                            break;
                    }
                    rawTransactions.Add(new RawTransaction(id, date, amount, transmitter, receiver));
                }
            }
        }

        /// <summary>
        /// Method to write Status of Accounts to file
        /// </summary>
        /// <param name="sttsAcctPath">
        /// Path of output Status of operation file
        /// </param>
        public void writeOperationsStatus(string sttsAcctPath)
        {
            using (StreamWriter sw = new StreamWriter(sttsAcctPath))
            {
                foreach (var rawAccount in rawAccounts)
                {
                    sw.WriteLine($"{rawAccount.Id};{rawAccount.Status}");
                }
            };
        }

        /// <summary>
        /// Method to write Account statistics
        /// </summary>
        /// <param name="mtrlPath">
        /// Path of statistics output file
        /// </param>
        public void writeStatistics(string mtrlPath)
        {
            using (StreamWriter sw = new StreamWriter(mtrlPath))
            {
                sw.WriteLine("Statistiques :");
                sw.WriteLine($"Nombre de comptes : {accounts.Count}");
                sw.WriteLine($"Nombre de transactions : {rawTransactions.Count}");
                sw.WriteLine($"Nombre de réussites : {validTransactionCount}");
                sw.WriteLine($"Nombre d'échecs : {rawTransactions.Count - validTransactionCount}");
                sw.WriteLine($"Montant total des réussites : {validTransactionsSum} euros");
                sw.WriteLine("");
                sw.WriteLine("Frais de gestions :");
                foreach (var fee in fees)
                {
                    sw.WriteLine($"{fee.Key} : {fee.Value} euros");
                }
            };
        }

        public void ProcessOperations()
        {
            DateTime temp;
            ILookup<DateTime, object> aLookup = rawAccounts.ToLookup(
                r =>
                {
                    DateTime.TryParseExact(
                           r.Date,
                           @"dd/MM/yyyy",
                           CultureInfo.InvariantCulture.DateTimeFormat,
                           DateTimeStyles.AllowWhiteSpaces,
                           out temp);
                    return temp;
                },
                r => (object)r);
            ILookup<DateTime, object> tLookup = rawTransactions.ToLookup(
                r =>
                {
                    DateTime.TryParseExact(
                           r.Date,
                           @"dd/MM/yyyy",
                           CultureInfo.InvariantCulture.DateTimeFormat,
                           DateTimeStyles.AllowWhiteSpaces,
                           out temp);
                    return temp;
                },
                r => (object)r);
            var mergedLookup = aLookup
                               .Concat(tLookup)
                               .SelectMany(lookup => lookup
                               .Select(value => new { lookup.Key, value }))
                               .ToLookup(x => x.Key, x => x.value);
            foreach (var group in mergedLookup.OrderBy(x => x.Key))
            {
                foreach (var obj in group)
                {
                    if (obj.GetType() == typeof(RawAccount))
                        ProcessAccount((RawAccount)obj);
                    if (obj.GetType() == typeof(RawTransaction))
                        ProcessTransaction((RawTransaction)obj);
                }
            }
        }

        /// <summary>
        /// Process raw client data into objects
        /// </summary>
        public void ProcessClients()
        {
            foreach (var rawClient in rawClients)
            {
                int id;
                int transactionCount;
                if (int.TryParse(rawClient.Id, out id)
                    && int.TryParse(rawClient.TransactionCount, out transactionCount)
                    && !clients.ContainsKey(id))
                {
                    if (rawClient.Type == "Particulier")
                    {
                        clients.Add(id, new Person(id, transactionCount));
                        rawClient.Status = Status.OK;
                    }
                    else if (rawClient.Type == "Entreprise")
                    {
                        clients.Add(id, new Company(id, transactionCount));
                        rawClient.Status = Status.OK;
                    }
                    else
                        rawClient.Status = Status.KO;
                }
                else
                    rawClient.Status = Status.KO;
            }
        }
        /// <summary>
        /// Process raw account data into objects
        /// </summary>
        public void ProcessAccount(RawAccount rawAccount)
        {
            int id;
            int entry;
            int exit;
            DateTime date;
            double balance = 0;

            //Check Id and Date validity 
            if (int.TryParse(rawAccount.Id, out id)
                && DateTime.TryParseExact(
                        rawAccount.Date,
                        @"dd/MM/yyyy",
                        CultureInfo.InvariantCulture.DateTimeFormat,
                        DateTimeStyles.AllowWhiteSpaces,
                        out date))
            {
                // Create account
                if (int.TryParse(rawAccount.Entry, out entry)
                    && rawAccount.Exit == string.Empty
                    && clients.ContainsKey(entry)
                    && int.TryParse(rawAccount.Id, out id)
                    && !accounts.ContainsKey(id))
                {
                    if (rawAccount.Balance == string.Empty
                        || (double.TryParse(rawAccount.Balance, out balance)
                            && balance >= 0))
                    {
                        Account account = new Account(id, date, entry, balance);
                        clients[entry].AddAccount(account);
                        accounts.Add(account.Id, account);
                        rawAccount.Status = Status.OK;
                    }
                }

                // Close account
                else if (int.TryParse(rawAccount.Exit, out exit)
                    && rawAccount.Entry == string.Empty
                    && clients.ContainsKey(exit)
                    && int.TryParse(rawAccount.Id, out id)
                    && clients[exit].Accounts.ContainsKey(id)
                    && clients[exit].Accounts[id].ClosureDate == null)
                {
                    clients[exit].Accounts[id].ClosureDate = date;
                    //accounts[id].ClosureDate = date;
                    rawAccount.Status = Status.OK;
                }

                // Transfer Account
                else if (int.TryParse(rawAccount.Exit, out exit)
                    && int.TryParse(rawAccount.Entry, out entry)
                    && clients.ContainsKey(exit)
                    && clients.ContainsKey(entry)
                    && int.TryParse(rawAccount.Id, out id)
                    && clients[entry].Accounts.ContainsKey(id)
                    && clients[entry].Accounts[id].ClosureDate == null)
                {
                    Account account = clients[entry].Accounts[id];
                    account.ClientId = exit;
                    clients[entry].Accounts.Remove(id);
                    clients[exit].Accounts.Add(id, account);
                    rawAccount.Status = Status.OK;
                }
                else
                    rawAccount.Status = Status.KO;
            }
            else
                rawAccount.Status = Status.KO;
        }
        /// <summary>
        /// Process raw transaction data into objects
        /// </summary>
        public void ProcessTransaction(RawTransaction rawTransaction)
        {
            int id;
            int transmitter;
            int receiver;
            DateTime date;
            double amount;

            if (int.TryParse(rawTransaction.Id, out id)
                && !transactions.ContainsKey(id)
                && DateTime.TryParseExact(
                        rawTransaction.Date,
                        @"dd/MM/yyyy",
                        CultureInfo.InvariantCulture.DateTimeFormat,
                        DateTimeStyles.AllowWhiteSpaces,
                        out date)
                && double.TryParse(rawTransaction.Amount, out amount)
                && int.TryParse(rawTransaction.Transmitter, out transmitter)
                && int.TryParse(rawTransaction.Receiver, out receiver))
            {
                Transaction transaction = new Transaction(
                        Transaction.TransactionType.Transfer,
                        id,
                        date,
                        amount,
                        transmitter,
                        receiver);

                transactions.Add(id, transaction);
                // Deposition
                if (transmitter == 0 && receiver != 0
                    && accounts.ContainsKey(receiver)
                    && clients[accounts[receiver].ClientId].Accounts[receiver].Deposit(transaction))
                {
                    transaction.Type = Transaction.TransactionType.Deposition;
                    rawTransaction.Status = Status.OK;
                    validTransactionCount++;
                    validTransactionsSum += amount;
                    fees.Add(id, 0);
                }
                //Withdrawal
                else if (receiver == 0 && transmitter != 0
                         && accounts.ContainsKey(transmitter)
                         && clients[accounts[transmitter].ClientId].Accounts[transmitter].Withdraw(transaction))
                {
                    transaction.Type = Transaction.TransactionType.Withdrawal;
                    rawTransaction.Status = Status.OK;
                    validTransactionCount++;
                    validTransactionsSum += amount;
                    fees.Add(id, 0);
                }
                // Transfer
                else if (receiver != transmitter
                         && accounts.ContainsKey(transmitter)
                         && accounts.ContainsKey(receiver)
                         && clients[accounts[receiver].ClientId].Accounts[receiver].CheckDeposit(transaction)
                         && clients[accounts[transmitter].ClientId].Accounts[transmitter].CheckWithdrawal(transaction))
                {
                    double fee = 0;
                    validTransactionsSum += amount;
                    // if transaction is external deduct transaction fee
                    if (clients[accounts[transmitter].ClientId].Id != clients[accounts[receiver].ClientId].Id
                        && clients[accounts[transmitter].ClientId].GetType() == typeof(Person))
                    {
                        fee = amount * ((Person)clients[accounts[transmitter].ClientId]).transactionFee;
                        amount -= fee;
                    }
                    if (clients[accounts[transmitter].ClientId].Id != clients[accounts[receiver].ClientId].Id
                        && clients[accounts[transmitter].ClientId].GetType() == typeof(Company))
                    {
                        fee = ((Person)clients[accounts[transmitter].ClientId]).transactionFee;
                        amount -= fee;
                    }

                    clients[accounts[transmitter].ClientId].Accounts[transmitter].Withdraw(transaction);
                    clients[accounts[receiver].ClientId].Accounts[receiver].Deposit(
                        new Transaction(
                            Transaction.TransactionType.Levy,
                            id,
                            date,
                            amount,
                            receiver,
                            transmitter));
                    rawTransaction.Status = Status.OK;
                    validTransactionCount++;
                    fees.Add(id, fee);

                }
                else
                    rawTransaction.Status = Status.KO;
            }
            else
                rawTransaction.Status = Status.KO;
        }

    }
}
