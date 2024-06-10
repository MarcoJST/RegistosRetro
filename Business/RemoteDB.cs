using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class RemoteDB
    {
        public static void CopyToRemoteDB()
        {
            var dbRemote = new Database.RegistosRetroRemote.RegistosRetroRemoteDB();

            if (!IsDatabaseConnected(dbRemote))
                return;

            var clients = Business.TClient.GetAll();

            foreach (var client in clients)
            {
                if (dbRemote.Debts.Where(x => x.idClient == client.id).Any())
                {
                    var debt = dbRemote.Debts.Where(x => x.idClient == client.id).Single();
                    debt.Debt = TClient.GetValueNotPaid(client.id);
                    debt.Date = DateTime.Now;
                }
                else
                {
                    dbRemote.Debts.Add(new Database.RegistosRetroRemote.Debts()
                    {
                        Date = DateTime.Now,
                        Debt = TClient.GetValueNotPaid(client.id),
                        Email = client.Email,
                        idClient = client.id,
                        NameClient = client.Name,
                        Phone = client.Phone
                    });
                }
            }

            dbRemote.SaveChanges();
        }

        private static bool IsDatabaseConnected(Database.RegistosRetroRemote.RegistosRetroRemoteDB dbRemote)
        {
            try
            {
                dbRemote.Database.Connection.Open();
                dbRemote.Database.Connection.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
