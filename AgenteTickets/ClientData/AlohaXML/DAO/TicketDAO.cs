using AgenteTickets.ClientData.AlohaXML.Models;
using AgenteTickets.Models;
using AgenteTickets.ClientData.Models;
using AgenteTickets.Utils;
using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;
using ACTools.NetFramework.Exceptions;
using AgenteTickets.AppDataBase.Models;
using ACTools.NetFramework.Files;

namespace AgenteTickets.ClientData.AlohaXML.DAO
{
    public static class TicketDAO
    {
        private static List<Item> FlattenItems(List<Item> items)
        {
            List<Item> result = new List<Item>();

            foreach (Item item in items)
            {
                result.Add(item);
                if (item.ChildItems.Any())
                {
                    result.AddRange(FlattenItems(item.ChildItems));
                }
            }

            return result;
        }

        public static List<DataInfo> Get(string path, string serie, List<PaymentMethodConfig> paymentMethodsConfig)
        {
            string fileExtension = ".xml";
            List<DataInfo> result = new List<DataInfo>();
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            FileInfo[] filesInfo = directoryInfo.GetFiles($"*{fileExtension}");

            foreach (FileInfo fileInfo in filesInfo)
            {
                DataInfo dataInfo = new DataInfo
                {
                    PathFileName = fileInfo.FullName,
                    FileExtension = fileExtension,
                };

                try
                {
                    Transacction transacction = FileConvert.XMLToObject<Transacction>(fileInfo.FullName);

                    dataInfo.NewFileName = $"{serie}{transacction.DOB:yyyyMMdd}{transacction.CheckNumber}{fileExtension}";

                    if (!transacction.IsClose)
                    {
                        throw new AppException("IsClose", "Ticket no cerrado.", 601);
                    }

                    Dictionary<string, decimal> paymentsTicket = new Dictionary<string, decimal>();

                    transacction.Tenders = transacction.Tenders.GroupBy(p => p.TenderID).Select(g => new Tender
                    {
                        TenderID = g.Key,
                        TenderAmount = g.Sum(p => p.TenderAmount)
                    }).ToList();

                    foreach (Tender tender in transacction.Tenders)
                    {
                        PaymentMethodConfig pmConfig = paymentMethodsConfig.Find(p => p.POSCode == tender.TenderID.ToString());

                        if (pmConfig != null)
                        {
                            if (paymentsTicket.ContainsKey(pmConfig.TaxCode))
                            {
                                paymentsTicket[pmConfig.TaxCode] += tender.TenderAmount;
                            }
                            else
                            {
                                paymentsTicket.Add(pmConfig.TaxCode, tender.TenderAmount);
                            }
                        }
                    }

                    if (!paymentsTicket.Any())
                    {
                        paymentsTicket = null;
                    }

                    dataInfo.Ticket = new Ticket
                    {
                        Serie = serie,
                        Folio = transacction.CheckNumber.ToString(),
                        Amount = transacction.Total,
                        Discount = Math.Abs(transacction.Comp),
                        Tip = 0,
                        TicketDate = transacction.DOB,
                        Guests = transacction.Guests,
                        Payment = paymentsTicket,
                        Details = FlattenItems(transacction.Items).Select(i =>
                        new TicketDetail
                        {
                            ProductOrServiceKey = i.ItemBOHName,
                            ProductId = i.ItemID.ToString(),
                            Quantity = (int)i.ItemQty,
                            UnitKey = i.ItemChitName2,
                            UnitName = "Servicio",
                            Description = i.ItemName,
                            UnitPrice = i.ItemUnitNetTotalCalculated + Math.Abs(i.ItemDiscountAmt),
                            Amount = i.ItemNetTotalCalculated + Math.Abs(i.ItemDiscountAmt),
                            Discount = Math.Abs(i.ItemDiscountAmt),
                        }).ToList()
                    };
                }
                catch (AppException ex)
                {
                    dataInfo.Code = ex.ErrorCode;
                    dataInfo.Message = $"{ex.Title}. {ex.Message}";
                }
                catch (Exception ex)
                {
                    dataInfo.NewFileName = fileInfo.Name;
                    dataInfo.Code = 600;
                    dataInfo.Message = $"Formato de archivo incorrecto. {ex.GetBaseException().Message}";
                }

                result.Add(dataInfo);
            }

            return result;
        }
    }
}
