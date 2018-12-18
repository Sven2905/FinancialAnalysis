using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Models;
using FinancialAnalysis.Models.Accounting;

namespace FinancialAnalysis.Logic
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

            using (var reader = new StreamReader(_FilePath))
            {
                var document = reader.ReadToEnd().Split('\n');

                foreach (var line in document)
                {
                    if (string.IsNullOrEmpty(line)) continue;

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
                    else
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

                    var taxTypes = db.TaxTypes.GetAll().ToList();

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
                    if (true)
                    {
                    }

                    var taxType = taxTypes.SingleOrDefault(x => x.DescriptionShort == _Content[6].Trim());
                    if (taxType != null)
                        costAccount.RefTaxTypeId = taxType.TaxTypeId;
                    else
                        costAccount.RefTaxTypeId = 1;

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
    }
}