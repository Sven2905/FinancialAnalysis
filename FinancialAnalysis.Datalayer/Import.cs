using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Models;
using FinancialAnalysis.Models.Accounting;
using FinancialAnalysis.Models.Administration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FinancialAnalysis.Datalayer
{
    public class Import
    {
        public void ImportCostAccounts(Standardkontenrahmen standardkontenrahmen)
        {
            var _FilePath = string.Empty;

            switch (standardkontenrahmen)
            {
                case Standardkontenrahmen.SKR03:
                    _FilePath = @".\Data\SKR03.txt";
                    CreateSKR03MainCategories();
                    break;
                case Standardkontenrahmen.SKR04:
                    _FilePath = @".\Data\SKR04.txt";
                    CreateSKR04MainCategories();
                    break;
            }

            var costAccountCategories = new List<CostAccountCategory>();

            var db = new DataLayer();
            costAccountCategories.AddRange(db.CostAccountCategories.GetAll());

            var taxTypes = new List<TaxType>
            {
                new TaxType
                {
                    DescriptionShort = "Keine", Description = "Keine", AmountOfTax = 0, TaxCategory = TaxCategory.None, TaxTypeId = 1
                },
                new TaxType
                {
                    DescriptionShort = "Bau 7%", Description = "Bau mit 7% USt/VSt", AmountOfTax = 7,
                    TaxCategory = TaxCategory.thirteenB, TaxTypeId = 2
                },
                new TaxType
                {
                    DescriptionShort = "I.g.E 7% USt/VSt", Description = "I.g.E 7% USt/VSt", AmountOfTax = 7,
                    TaxCategory = TaxCategory.igE, TaxTypeId = 3
                },
                new TaxType
                {
                    DescriptionShort = "I.g.E 16% USt/VSt", Description = "I.g.E 16% USt/VSt", AmountOfTax = 16,
                    TaxCategory = TaxCategory.igE, TaxTypeId = 4
                },
                new TaxType
                {
                    DescriptionShort = "I.g.E 19% USt/VSt", Description = "I.g.E 19% USt/VSt", AmountOfTax = 19,
                    TaxCategory = TaxCategory.igE, TaxTypeId = 5
                },
                new TaxType
                {
                    DescriptionShort = "I.g.E Neufahrzeug", Description = "I.g.E Neufahrzeuge 19% USt/VSt",
                    AmountOfTax = 19, TaxCategory = TaxCategory.igE, TaxTypeId = 6
                },
                new TaxType
                {
                    DescriptionShort = "Kfz 19% VSt. 50%", Description = "Kfz 19% Vorsteuer. 50%", AmountOfTax = 19,
                    TaxCategory = TaxCategory.fiftyPercent, TaxTypeId = 7
                },
                new TaxType
                {
                    DescriptionShort = "Kfz VSt. 50%", Description = "Kfz Vorsteuer. 50%", AmountOfTax = 16,
                    TaxCategory = TaxCategory.fiftyPercent, TaxTypeId = 8
                },
                new TaxType
                {
                    DescriptionShort = "USt. 15%", Description = "Umsatzsteuer 15%", AmountOfTax = 15,
                    TaxCategory = TaxCategory.Netto, TaxTypeId = 9
                },
                new TaxType
                {
                    DescriptionShort = "USt. 16%", Description = "Umsatzsteuer 16%", AmountOfTax = 16,
                    TaxCategory = TaxCategory.Netto, TaxTypeId = 10
                },
                new TaxType
                {
                    DescriptionShort = "USt. 19%", Description = "Umsatzsteuer 19%", AmountOfTax = 19,
                    TaxCategory = TaxCategory.Netto, TaxTypeId = 11
                },
                new TaxType
                {
                    DescriptionShort = "USt. 7%", Description = "Umsatzsteuer 7%", AmountOfTax = 7,
                    TaxCategory = TaxCategory.Netto, TaxTypeId = 12
                },
                new TaxType
                {
                    DescriptionShort = "USt/VSt 19%",
                    Description = "Reverse Charge (Steuerschuld Leistungsempf.) 19% USt/VSt", AmountOfTax = 19,
                    TaxCategory = TaxCategory.thirteenB, TaxTypeId = 13
                },
                new TaxType
                {
                    DescriptionShort = "USt/VSt 7%",
                    Description = "Reverse Charge (Steuerschuld Leistungsempf.) 7% USt/VSt", AmountOfTax = 7,
                    TaxCategory = TaxCategory.thirteenB, TaxTypeId = 14
                },
                new TaxType
                {
                    DescriptionShort = "VSt. 15%", Description = "Vorsteuer 15%", AmountOfTax = 15,
                    TaxCategory = TaxCategory.Netto, TaxTypeId = 15
                },
                new TaxType
                {
                    DescriptionShort = "VSt. 16%", Description = "Vorsteuer 16%", AmountOfTax = 16,
                    TaxCategory = TaxCategory.Netto, TaxTypeId = 16
                },
                new TaxType
                {
                    DescriptionShort = "VSt. 19%", Description = "Vorsteuer 19%", AmountOfTax = 19,
                    TaxCategory = TaxCategory.Netto, TaxTypeId = 17
                },
                new TaxType
                {
                    DescriptionShort = "VSt. 7%", Description = "Vorsteuer 7%", AmountOfTax = 7,
                    TaxCategory = TaxCategory.Netto, TaxTypeId = 18
                }
            };

            using (var reader = new StreamReader(_FilePath))
            {
                var document = reader.ReadToEnd().Split('\n');

                foreach (var line in document)
                {
                    if (string.IsNullOrEmpty(line))
                    {
                        continue;
                    }

                    // [0] Konto-Nummer	
                    // [1] Kontenbezeichnung	
                    // [2] Kontenkategorie	
                    // [3] Kontenunterart	
                    // [4] USt.Pos	
                    // [5] USt.	
                    // [6] Steuer	
                    // [7] USt.PosE	
                    // [8] Zuordnung EÜ	
                    // [9] Zuordnung Aktiva	
                    // [10] Zuordnung Passiva	
                    // [11] Zuordnung GuV

                    var _Content = line.Split('\t');

                    var mainCatId = 0;
                    var subCatId = 0;

                    var tempMainCat = new CostAccountCategory();

                    // Get parent category
                    if (standardkontenrahmen == Standardkontenrahmen.SKR03)
                    {
                        switch (_Content[0][0])
                        {
                            case '0':
                                tempMainCat.ParentCategoryId = 1;
                                break;
                            case '1':
                                tempMainCat.ParentCategoryId = 2;
                                break;
                            case '2':
                                tempMainCat.ParentCategoryId = 3;
                                break;
                            case '3':
                                tempMainCat.ParentCategoryId = 4;
                                break;
                            case '4':
                                tempMainCat.ParentCategoryId = 5;
                                break;
                            case '7':
                                tempMainCat.ParentCategoryId = 6;
                                break;
                            case '8':
                                tempMainCat.ParentCategoryId = 7;
                                break;
                            case '9':
                                tempMainCat.ParentCategoryId = 8;
                                break;
                        }
                    }
                    else
                    {
                        switch (_Content[0][0])
                        {
                            case '0':
                                tempMainCat.ParentCategoryId = 1;
                                break;
                            case '1':
                                tempMainCat.ParentCategoryId = 2;
                                break;
                            case '2':
                                tempMainCat.ParentCategoryId = 3;
                                break;
                            case '3':
                                tempMainCat.ParentCategoryId = 4;
                                break;
                            case '4':
                                tempMainCat.ParentCategoryId = 5;
                                break;
                            case '5':
                                tempMainCat.ParentCategoryId = 6;
                                break;
                            case '6':
                                tempMainCat.ParentCategoryId = 7;
                                break;
                            case '7':
                                tempMainCat.ParentCategoryId = 8;
                                break;
                            case '9':
                                tempMainCat.ParentCategoryId = 9;
                                break;
                        }
                    }

                    if (costAccountCategories.SingleOrDefault(x =>
                            x.Description == _Content[2] && x.ParentCategoryId == tempMainCat.ParentCategoryId) == null)
                    {
                        tempMainCat.Description = _Content[2];

                        mainCatId = db.CostAccountCategories.Insert(tempMainCat);
                        tempMainCat.CostAccountCategoryId = mainCatId;
                        costAccountCategories.Add(tempMainCat);
                    }
                    else
                    {
                        mainCatId = costAccountCategories.SingleOrDefault(x =>
                                x.Description == _Content[2] && x.ParentCategoryId == tempMainCat.ParentCategoryId)
                            .CostAccountCategoryId;
                    }

                    var tempSubCat = new CostAccountCategory();

                    if (!string.IsNullOrEmpty(_Content[3]))
                    {
                        if (costAccountCategories.SingleOrDefault(x =>
                                x.Description == _Content[3] && x.ParentCategoryId == mainCatId) == null)
                        {
                            tempSubCat.Description = _Content[3];
                            tempSubCat.ParentCategoryId = mainCatId;

                            subCatId = db.CostAccountCategories.Insert(tempSubCat);
                            tempSubCat.CostAccountCategoryId = subCatId;
                            costAccountCategories.Add(tempSubCat);
                        }
                        else
                        {
                            subCatId = costAccountCategories
                                .SingleOrDefault(x => x.Description == _Content[3] && x.ParentCategoryId == mainCatId)
                                .CostAccountCategoryId;
                        }
                    }
                    else
                    {
                        subCatId = mainCatId;
                    }


                    var costAccount = new CostAccount
                    {
                        AccountNumber = Convert.ToInt32(_Content[0]),
                        RefCostAccountCategoryId = subCatId,
                        Description = _Content[1],
                        IsVisible = true,
                        IsEditable = false
                    };

                    var taxType = taxTypes.SingleOrDefault(x => x.DescriptionShort == _Content[6].Trim());
                    if (taxType != null)
                    {
                        costAccount.RefTaxTypeId = taxType.TaxTypeId;
                    }
                    else
                    {
                        costAccount.RefTaxTypeId = 1;
                    }

                    db.CostAccounts.Insert(costAccount);
                }
            }
        }

        private void CreateSKR03MainCategories()
        {
            var _CostAccounts = new List<CostAccountCategory>
            {
                new CostAccountCategory {Description = " 0 - Anlage- und Kapitalkonten"},
                new CostAccountCategory {Description = " 1 - Finanz- und Privatkonten"},
                new CostAccountCategory {Description = " 2 - Abgrenzungskonten"},
                new CostAccountCategory {Description = " 3 - Wareneingangs- und Bestandkonten"},
                new CostAccountCategory {Description = " 4 - Betriebliche Aufwendungen"},
                new CostAccountCategory {Description = " 7 - Bestände an Erzeugnissen"},
                new CostAccountCategory {Description = " 8 - Erlöskonten"},
                new CostAccountCategory {Description = " 9 - Vortrags-, Kapital- und statistische Konten"}
            };

            var db = new DataLayer();
            db.CostAccountCategories.Insert(_CostAccounts);
        }

        private void CreateSKR04MainCategories()
        {
            var _CostAccounts = new List<CostAccountCategory>
            {
                new CostAccountCategory {Description = " 0 - Anlagevermögen"},
                new CostAccountCategory {Description = " 1 - Umlaufvermögen"},
                new CostAccountCategory {Description = " 2 - Eigenkapitalkonten"},
                new CostAccountCategory {Description = " 3 - Fremdkapitalkonten"},
                new CostAccountCategory {Description = " 4 - Betriebliche Erträge"},
                new CostAccountCategory {Description = " 5 - Betriebliche Aufwendungen"},
                new CostAccountCategory {Description = " 6 - Betriebliche Aufwendungen"},
                new CostAccountCategory {Description = " 7 - Weitere Erträge und Aufwendungen"},
                new CostAccountCategory {Description = " 9 - Vortrags-, Kapital- und statistische Konten"}
            };

            var db = new DataLayer();
            db.CostAccountCategories.Insert(_CostAccounts);
        }

        public void ImportUserRights()
        {
            List<UserRight> rights = new List<UserRight>()
            {
                new UserRight(Permission.AccessAccounting, "Buchhaltung", 0, "Erlaubt den Zugriff auf den Menüpunkt Buchhaltung"),                                  
                new UserRight(Permission.AccessProjectManagement, "Projektmanagement", 0, "Erlaubt den Zugriff auf den Menüpunkt Projektmanagement"),               
                new UserRight(Permission.AccessConfiguration, "Konfiguration", 0, "Erlaubt den Zugriff auf den Menüpunkt Konfiguration"),                           
                new UserRight(Permission.AccessStockManagement, "Lagerverwaltung", 0, "Erlaubt den Zugriff auf den Menüpunkt Lagerverwaltung"),                     

                new UserRight(Permission.AccessBooking, "Buchungen", (int)Permission.AccessAccounting, "Erlaubt den Zugriff auf den Menüpunkt Buchungen"),                                         
                new UserRight(Permission.AccessBookingHistory, "Buchungshistorie", (int)Permission.AccessAccounting, "Erlaubt den Zugriff auf den Menüpunkt Buchungshistorie"),                    
                new UserRight(Permission.AccessCreditorDebitor, "Kreditoren und Debitoren", (int)Permission.AccessAccounting, "Erlaubt den Zugriff auf den Menüpunkt Kreditoren und Debitoren"),   
                new UserRight(Permission.AccessTaxType, "Steuersätze", (int)Permission.AccessAccounting, "Erlaubt den Zugriff auf den Menüpunkt Steuersätze"),                                     
                new UserRight(Permission.AccessCostAccount, "Kontenrahmen", (int)Permission.AccessAccounting, "Erlaubt den Zugriff auf den Menüpunkt Kontenrahmen"),                               
                new UserRight(Permission.AccessCostCenter, "Kostenstellen", (int)Permission.AccessProjectManagement, "Erlaubt den Zugriff auf den Menüpunkt Kostenstellen"),                              
                new UserRight(Permission.AccessEmployee, "Mitarbeiter", (int)Permission.AccessProjectManagement, "Erlaubt den Zugriff auf den Menüpunkt Mitarbeiter"),                                    
                new UserRight(Permission.AccessProject, "Projekte", (int)Permission.AccessProjectManagement, "Erlaubt den Zugriff auf den Menüpunkt Projekte"),                                           
                new UserRight(Permission.AccessProjectWorkingTime, "Zeiterfassungen", (int)Permission.AccessProjectManagement, "Erlaubt den Zugriff auf den Menüpunkt Zeiterfassungen"),                  
                new UserRight(Permission.AccessMail, "Mailkonfiguration", (int)Permission.AccessConfiguration, "Erlaubt den Zugriff auf den Menüpunkt Mailkonfiguration"),                            
                new UserRight(Permission.AccessUsers, "Benutzer", (int)Permission.AccessConfiguration, "Erlaubt den Zugriff auf den Menüpunkt Benutzer"),                                             
                new UserRight(Permission.AccessPaymentCondidition, "Zahlungsbedingungen", (int)Permission.AccessAccounting, "Erlaubt den Zugriff auf den Menüpunkt Zahlungsbedingungen"),          
            };

            using (DataLayer db = new DataLayer())
            {
                foreach (var item in rights)
                {
                    item.UserRightId = db.UserRights.Insert(item);
                    db.UserRightUserMappings.Insert(new UserRightUserMapping() { RefUserId = 1, RefUserRightId = item.UserRightId, IsGranted = true });
                }
            }
        }
    }
}