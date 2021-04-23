using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banque
{
    class Utils
    {
        Dictionary<int, Account> accounts = new Dictionary<int, Account>();
        Dictionary<int, Transaction> transactions = new Dictionary<int, Transaction>();

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
                    int id;
                    double balance = 0;
                    string sBalance = "";
                    string[] data = sr.ReadLine().Split(';');
                    if (data.Length > 0)
                    {
                        string sId = data[0];
                        if (data.Length > 1)
                        {
                            sBalance = data[1].Trim();
                        }
                        // Account id is valid and not already used
                        if (int.TryParse(sId, out id) && !accounts.ContainsKey(id))
                        {
                            if (sBalance == "")
                                accounts.Add(id, new Account(id));
                            else if (double.TryParse(
                                        sBalance,
                                        //System.Globalization.NumberStyles.AllowDecimalPoint,
                                        //System.Globalization.NumberFormatInfo.InvariantInfo,
                                        out balance)
                                    && balance >= 0)
                                accounts.Add(id, new Account(id, balance));
                        }
                    }
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
                int invalidId = 0;
                while (!sr.EndOfStream)
                {
                    string rawData = sr.ReadLine();
                    string[] data = rawData.Split(';');
                    string sId = data[0];
                    string sAmount = data[1];
                    string sTransmitter = data[2];
                    string sRecipient = data[3];
                    int id;
                    double amount;
                    int transmitter;
                    int recipient;
                    Transaction transaction;
                    // Transaction data is valid and id is not already used
                    if (int.TryParse(sId, out id) &&
                        double.TryParse(sAmount, out amount) &&
                        int.TryParse(sTransmitter, out transmitter) &&
                        int.TryParse(sRecipient, out recipient) &&
                        !transactions.ContainsKey(id))
                    {
                        transaction = new Transaction(
                                Transaction.TransactionType.Transfer,
                                Transaction.TransactionStatus.OK,
                                sId,
                                "",
                                id,
                                amount,
                                transmitter,
                                recipient);

                        if (transmitter == 0 && recipient != 0)
                        {
                            transaction.Type = Transaction.TransactionType.Deposition;
                        }
                        else if (recipient == 0 && transmitter != 0)
                        {
                            transaction.Type = Transaction.TransactionType.Withdrawal;
                        }
                        else if (recipient == 0 && transmitter == 0)
                            transaction.Status = Transaction.TransactionStatus.KO;
                        transactions.Add(id, transaction);

                    }
                    // At least transaction id is valid and id is not already used
                    else if (int.TryParse(sId, out id) && !transactions.ContainsKey(id))
                    {
                        transaction = new Transaction(
                            Transaction.TransactionType.Transfer,
                            Transaction.TransactionStatus.KO,
                            sId,
                            rawData,
                            id);
                        transactions.Add(id, transaction);
                    }
                    // Transaction id is invalid or already used
                    else
                    {
                        transaction = new Transaction(
                            Transaction.TransactionType.Transfer,
                            Transaction.TransactionStatus.KO,
                            sId,
                            rawData);
                        transactions.Add(--invalidId, transaction);
                    }
                }
            }
        }

        /// <summary>
        /// Method to write Status of Transactions to file
        /// </summary>
        /// <param name="sttsTrxnPath">
        /// Path of output Status of operation file
        /// </param>
        public void writeTransactionsStatus(string sttsTrxnPath)
        {
            using (StreamWriter sw = new StreamWriter(sttsTrxnPath))
            {
                TryTransactions();
                foreach (var t in transactions)
                {
                    if (t.Key > 0)
                        sw.WriteLine($"{t.Key};{t.Value.Status}");
                    else
                        sw.WriteLine($"{t.Value.OriginalId};{t.Value.Status}");
                }
            };
        }

        /// <summary>
        /// Method to execute transaction and return status
        /// </summary>
        private void TryTransactions()
        {
            Console.Write("     ");
            foreach (var account in accounts)
            {
                Console.Write($"{account.Value.Balance.ToString("F")} ");
            }
            Console.WriteLine();

            foreach (var t in transactions)
            {
                if (t.Value.Status == Transaction.TransactionStatus.OK)
                {
                    if (t.Value.Type == Transaction.TransactionType.Deposition)
                    {
                        if (accounts.ContainsKey(t.Value.Recipient))
                        {
                            if (accounts[t.Value.Recipient].Deposit(t.Value.Amount, t.Value))
                                t.Value.Status = Transaction.TransactionStatus.OK;
                            else
                                t.Value.Status = Transaction.TransactionStatus.KO;
                        }
                        else
                            t.Value.Status = Transaction.TransactionStatus.KO;

                    }
                    if (t.Value.Type == Transaction.TransactionType.Withdrawal)
                    {
                        if (accounts.ContainsKey(t.Value.Transmitter))
                        {
                            if (accounts[t.Value.Transmitter].Withdraw(t.Value.Amount, t.Value))
                                t.Value.Status = Transaction.TransactionStatus.OK;
                            else
                                t.Value.Status = Transaction.TransactionStatus.KO;
                        }
                        else
                            t.Value.Status = Transaction.TransactionStatus.KO;
                    }
                    if (t.Value.Type == Transaction.TransactionType.Transfer)
                    {
                        if (accounts.ContainsKey(t.Value.Transmitter) &&
                            accounts.ContainsKey(t.Value.Recipient) &&
                            t.Value.Recipient != t.Value.Transmitter)
                        {
                            if (t.Value.Amount > 0 && accounts[t.Value.Transmitter].Withdraw(t.Value.Amount, t.Value))
                            {
                                Transaction transaction = new Transaction(
                                    Transaction.TransactionType.Levy,
                                    Transaction.TransactionStatus.OK,
                                    t.Value.OriginalId,
                                    "",
                                    t.Value.Id,
                                    t.Value.Amount,
                                    t.Value.Recipient,
                                    t.Value.Transmitter);
                                accounts[t.Value.Recipient].Deposit(t.Value.Amount, transaction);
                                t.Value.Status = Transaction.TransactionStatus.OK;
                            }
                            else
                                t.Value.Status = Transaction.TransactionStatus.KO;
                        }
                        else
                            t.Value.Status = Transaction.TransactionStatus.KO;
                    }
                }
                if (t.Key > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append($"{t.Key} {t.Value.Status} ");
                    foreach (var account in accounts)
                    {
                        sb.Append($"{account.Value.Balance.ToString("F")} ");
                    }
                    Console.WriteLine(sb.ToString());
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append($"{t.Value.OriginalId} {t.Value.Status} ");
                    foreach (var account in accounts)
                    {
                        sb.Append($"{account.Value.Balance.ToString("F")} ");
                    }
                    Console.WriteLine(sb.ToString());
                }
            }
        }
    }
}
