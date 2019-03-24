using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Models;
using FinancialAnalysis.Models.Accounting;
using FinancialAnalysis.Models.Administration;
using FinancialAnalysis.Models.CarPoolManagement;
using FinancialAnalysis.Models.ClientManagement;
using FinancialAnalysis.Models.Enums;
using FinancialAnalysis.Models.ProductManagement;
using FinancialAnalysis.Models.ProjectManagement;
using FinancialAnalysis.Models.SalesManagement;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FinancialAnalysis.Logic
{
    public class Import
    {
        private List<BalanceAccount> BalanceAccounts;
        private List<GainAndLossAccount> GainAndLossAccounts;

        public void SeedGainAndLossAccounts()
        {
            GainAndLossAccounts = new List<GainAndLossAccount>
            {
                new GainAndLossAccount(1, "Umsatzerlöse"),
                new GainAndLossAccount(2, "Bestandsveränderungen fertige/unfertige Erzeugnisse"),
                new GainAndLossAccount(3, "Aktivierte Eigenleistungen"),
                new GainAndLossAccount(4, "Sonstige betriebliche Erträge"),
                new GainAndLossAccount(5, "Materialaufwand"),
                new GainAndLossAccount(6, "Personalaufwand"),
                new GainAndLossAccount(7, "Abschreibungen"),
                new GainAndLossAccount(8, "Sonstige betriebliche Aufwendungen"),
                new GainAndLossAccount(9, "Raumkosten", 8),
                new GainAndLossAccount(10, "Versicherungen, Beiträge und Abgaben", 8),
                new GainAndLossAccount(11, "Reparaturen und Instandhaltungen", 8),
                new GainAndLossAccount(12, "Fahrzeugkosten", 8),
                new GainAndLossAccount(13, "Werbe- und Reisekosten", 8),
                new GainAndLossAccount(14, "Kosten der Warenabgabe", 8),
                new GainAndLossAccount(15, "Verschiedene betriebliche Kosten", 8),
                new GainAndLossAccount(16, "Erträge aus Beteiligungen"),
                new GainAndLossAccount(17, "Erträge aus anderen Wertpapieren u. Ausleihungen"),
                new GainAndLossAccount(18, "Sonstige Zinsen und ähnliche Erträge"),
                new GainAndLossAccount(19, "Abschreibungen auf Finanzanlagen u. Wertpapiere des UV"),
                new GainAndLossAccount(20, "Zinsen und ähnliche Aufwendungen"),
                new GainAndLossAccount(21, "Steuern vom Einkommen und Ertrag"),
                new GainAndLossAccount(22, "Sonstige Steuern")
            };

            for (int i = 0; i < GainAndLossAccounts.Count; i++)
            {
                DataContext.Instance.GainAndLossAccounts.Insert(GainAndLossAccounts[i]);
            }
        }

        public void SeedBalanceAccounts()
        {
            BalanceAccounts = new List<BalanceAccount>
                {
                    // AKTIVA
                    new BalanceAccount(1, "A. Anlagevermögen", AccountType.Active, 0),
                    new BalanceAccount(2, "I. Immaterielle Vermögensgegenstände", AccountType.Active, 1),
                    new BalanceAccount(3, "1. Selbst geschaffene gewerbliche Schutzrechte und ähnliche Rechte und Werte", AccountType.Active, 2),
                    new BalanceAccount(4, "2. Entgeltlich erworbene Konzessionen, gewerbliche Schutzrechte und ähnliche Rechte und Werte sowie Lizenzen an solchen Rechten und Werten", AccountType.Active, 2),
                    new BalanceAccount(5, "3. Geschäfts- oder Firmenwert", AccountType.Active, 2),
                    new BalanceAccount(6, "4. Geleistete Anzahlungen", AccountType.Active, 2),
                    new BalanceAccount(7, "II. Sachanlagen", AccountType.Active, 1),
                    new BalanceAccount(8, "1. Grundstücke, grundstücksgleiche Rechte und Bauten einschließlich der Bauten auf fremden Grundstücken", AccountType.Active, 7),
                    new BalanceAccount(9, "2. Technische Anlagen und Maschinen", AccountType.Active, 7),
                    new BalanceAccount(10, "3. Andere Anlagen, Betriebs-und Geschäftsausstattung", AccountType.Active, 7),
                    new BalanceAccount(11, "4. Geleistete Anzahlungen und Anlagen im Bau", AccountType.Active, 7),
                    new BalanceAccount(12, "III. Finanzanlagen", AccountType.Active, 1),
                    new BalanceAccount(13, "1. Anteile an verbundenen Unternehmen Finanzanlagen", AccountType.Active, 12),
                    new BalanceAccount(14, "2. Ausleihungen an verbundene Unternehmen", AccountType.Active, 12),
                    new BalanceAccount(15, "3. Beteiligungen", AccountType.Active, 12),
                    new BalanceAccount(16, "4. Ausleihungen an Unternehmen, mit denen ein Beteiligungsverhältnis besteht", AccountType.Active, 12),
                    new BalanceAccount(17, "5. Wertpapiere des Anlagevermögens", AccountType.Active, 12),
                    new BalanceAccount(18, "6. Sonstige Ausleihungen", AccountType.Active, 12),
                    new BalanceAccount(19, "B. Umlaufvermögen", AccountType.Active, 0),
                    new BalanceAccount(20, "I. Vorräte", AccountType.Active, 19),
                    new BalanceAccount(21, "1. Roh-, Hilfs- und Betriebsstoffe", AccountType.Active, 20),
                    new BalanceAccount(22, "2. Unfertige Erzeugnisse, unfertige Leistungen", AccountType.Active, 20),
                    new BalanceAccount(23, "3. Fertige Erzeugnisse und Waren", AccountType.Active, 20),
                    new BalanceAccount(24, "4. Geleistete Anzahlungen auf Vorräte", AccountType.Active, 20),
                    new BalanceAccount(25, "II. Forderungen und sonstige Vermögensgegenstände", AccountType.Active, 19),
                    new BalanceAccount(26, "1. Forderungen aus Lieferungen und Leistungen", AccountType.Active, 25),
                    new BalanceAccount(27, "2. Forderungen gegen verbundene Unternehmen", AccountType.Active, 25),
                    new BalanceAccount(28, "3. Forderungen gegen Unternehmen, mit denen ein Beteiligungsverhältnis besteht", AccountType.Active, 25),
                    new BalanceAccount(29, "4. sonstige Vermögensgegenstände", AccountType.Active, 25),
                    new BalanceAccount(30, "III. Wertpapiere", AccountType.Active, 19),
                    new BalanceAccount(31, "1. Anteile an verbundenen Unternehmen Wertpapiere", AccountType.Active, 30),
                    new BalanceAccount(32, "2. Sonstige Wertpapiere", AccountType.Active, 30),
                    new BalanceAccount(33, "IV. Kassenbestand, Bundesbankguthaben, Guthaben bei Kreditinstituten und Schecks", AccountType.Active, 19),
                    new BalanceAccount(34, "C. Rechnungsabgrenzungsposten", AccountType.Active, 0),
                    new BalanceAccount(35, "D. Aktive latente Steuern", AccountType.Active, 0),
                    new BalanceAccount(36, "E. Aktiver Unterschiedsbetrag aus der Vermögensverrechnung", AccountType.Active, 0),

                    // PASSIVA
                    new BalanceAccount(37, "A. Eigenkapital", AccountType.Passive, 0),
                    new BalanceAccount(38, "I. Kapital", AccountType.Passive, 37),
                    new BalanceAccount(39, "1. Gezeichnetes Kapital", AccountType.Passive, 38),
                    new BalanceAccount(40, "Ausstehende Einlagen auf das gezeichnete Kapital", AccountType.Passive, 39),
                    new BalanceAccount(41, "2. Variables Kapital", AccountType.Passive, 38),
                    new BalanceAccount(42, "Einlagen/Entnahmen", AccountType.Passive, 41),
                    new BalanceAccount(43, "II. Kapitalrücklagen", AccountType.Passive, 37),
                    new BalanceAccount(44, "III. Gewinnrücklagen", AccountType.Passive, 37),
                    new BalanceAccount(45, "1. Gesetzliche Rücklagen", AccountType.Passive, 44),
                    new BalanceAccount(46, "2. Rücklage für Anteile an einem herrschenden oder mehrheitlich beteiligten Unternehmen", AccountType.Passive, 44),
                    new BalanceAccount(47, "3. Satzungsmäßige Rücklagen", AccountType.Passive, 44),
                    new BalanceAccount(48, "4. Andere Gewinnrücklagen", AccountType.Passive, 44),
                    new BalanceAccount(49, "IV. Gewinn- und Verlustvortrag", AccountType.Passive, 37),
                    new BalanceAccount(50, "V. Jahresüberschuß / Jahresfehlbetrag", AccountType.Passive, 37),
                    new BalanceAccount(51, "B. Rückstellungen", AccountType.Passive, 0),
                    new BalanceAccount(52, "1. Rückstellungen für Pensionen und ähnliche Verpflichtungen", AccountType.Passive, 51),
                    new BalanceAccount(53, "2. Steuerrückstellungen", AccountType.Passive, 51),
                    new BalanceAccount(54, "3. Sonstige Rückstellungen", AccountType.Passive, 51),
                    new BalanceAccount(55, "C. Verbindlichkeiten", AccountType.Passive, 0),
                    new BalanceAccount(56, "1. Anleihen", AccountType.Passive, 55),
                    new BalanceAccount(57, "2. Verbindlichkeiten gegenüber Kreditinstituten", AccountType.Passive, 55),
                    new BalanceAccount(58, "3. Erhaltene Anzahlungen auf Bestellungen", AccountType.Passive, 55),
                    new BalanceAccount(59, "4. Verbindlichkeiten aus Lieferungen und Leistungen", AccountType.Passive, 55),
                    new BalanceAccount(60, "5. Verbindlichkeiten aus der Annahme gezogener Wechsel und der Ausstellung eigener Wechsel", AccountType.Passive, 55),
                    new BalanceAccount(61, "6. Verbindlichkeiten gegenüber verbundenen Unternehmen", AccountType.Passive, 55),
                    new BalanceAccount(62, "7. Verbindlichkeiten gegenüber Unternehmen, mit denen ein Beteiligungsverhältnis besteht", AccountType.Passive, 55),
                    new BalanceAccount(63, "8. Sonstige Verbindlichkeiten", AccountType.Passive, 55),
                    new BalanceAccount(64, "D. Rechnungsabgrenzungsposten", AccountType.Passive, 0),
                    new BalanceAccount(65, "E. Passive latente Steuern", AccountType.Passive, 0)
            };

            for (int i = 0; i < BalanceAccounts.Count; i++)
            {
                DataContext.Instance.BalanceAccounts.Insert(BalanceAccounts[i]);
            }
        }

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

            var taxTypes = DataContext.Instance.TaxTypes.GetAll().ToList();
            var costAccountCategories = new List<CostAccountCategory>();

            costAccountCategories.AddRange(DataContext.Instance.CostAccountCategories.GetAll());

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

                        mainCatId = DataContext.Instance.CostAccountCategories.Insert(tempMainCat);
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

                            subCatId = DataContext.Instance.CostAccountCategories.Insert(tempSubCat);
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

                    // Aktiva Content[9]
                    int aktivaId = 0;
                    // Passiva Content[10]
                    int passivaId = 0;
                    // GuV Content[11]
                    int gainAndLossAccountId = 0;

                    if (BalanceAccounts.SingleOrDefault(x => string.Equals(x.Name, _Content[9], StringComparison.OrdinalIgnoreCase)) != null)
                    {
                        aktivaId = BalanceAccounts.Single(x => string.Equals(x.Name, _Content[9], StringComparison.OrdinalIgnoreCase)).BalanceAccountId;
                    }

                    if (BalanceAccounts.SingleOrDefault(x => string.Equals(x.Name, _Content[10], StringComparison.OrdinalIgnoreCase)) != null)
                    {
                        passivaId = BalanceAccounts.Single(x => string.Equals(x.Name, _Content[10], StringComparison.OrdinalIgnoreCase)).BalanceAccountId;
                    }

                    if (GainAndLossAccounts.SingleOrDefault(x => string.Equals(x.Name, _Content[11].Replace("\r", "").Trim(), StringComparison.OrdinalIgnoreCase)) != null)
                    {
                        gainAndLossAccountId = GainAndLossAccounts.Single(x => string.Equals(x.Name, _Content[11].Replace("\r", "").Trim(), StringComparison.OrdinalIgnoreCase)).GainAndLossAccountId;
                    }

                    var costAccount = new CostAccount
                    {
                        AccountNumber = Convert.ToInt32(_Content[0]),
                        RefCostAccountCategoryId = subCatId,
                        Description = _Content[1],
                        IsVisible = true,
                        IsEditable = false,
                        RefActiveBalanceAccountId = aktivaId,
                        RefPassiveBalanceAccountId = passivaId,
                        RefGainAndLossAccountId = gainAndLossAccountId
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

                    DataContext.Instance.CostAccounts.Insert(costAccount);
                }
            }
        }

        public void SeedCars()
        {
            Dictionary<string, int> makes = new Dictionary<string, int>();
            List<CarMake> carMakes = new List<CarMake>();
            Dictionary<string, int> models = new Dictionary<string, int>();
            List<CarModel> carModels = new List<CarModel>();
            Dictionary<string, int> generations = new Dictionary<string, int>();
            List<CarGeneration> carGenerations = new List<CarGeneration>();
            Dictionary<string, int> bodies = new Dictionary<string, int>();
            List<CarBody> carBodies = new List<CarBody>();
            List<CarTrim> carTrims = new List<CarTrim>();
            List<CarEngine> carEngines = new List<CarEngine>();
            List<CarModelBodyMapping> carModelBodyMappings = new List<CarModelBodyMapping>();
            HashSet<string> modelBodyMappings = new HashSet<string>();
            int makeCounter = 1;
            int modelCounter = 1;
            int generationCounter = 1;
            int bodyCounter = 1;
            int trimCounter = 1;


            const string _FilePath = @".\Data\auto_databases_February_2019.csv";

            using (var reader = new StreamReader(_FilePath))
            {
                var lineCounter = 0;
                foreach (var line in reader.ReadToEnd().Split('\n'))
                {
                    if (string.IsNullOrEmpty(line))
                    {
                        continue;
                    }

                    lineCounter++;
                    if (lineCounter > 1)
                    {
                        var values = line.Split(';');

                        // MARKE
                        if (!makes.Keys.Contains(values[3]))
                        {
                            CarMake carMake = new CarMake { Name = values[3] };
                            carMakes.Add(carMake);
                            makes.Add(carMake.Name, makeCounter++);
                        }

                        // BAUART
                        if (!bodies.Keys.Contains(values[9]))
                        {
                            CarBody carBody = new CarBody { Name = values[9] };
                            carBodies.Add(carBody);
                            bodies.Add(carBody.Name, bodyCounter++);
                        }

                        // MODEL
                        if (!models.Keys.Contains(values[5]))
                        {
                            CarModel carModel = new CarModel { Name = values[5], RefCarMakeId = makes[values[3]] };
                            carModels.Add(carModel);
                            models.Add(carModel.Name, modelCounter++);
                        }

                        // GENERATION
                        if (!generations.Keys.Contains(values[7]))
                        {
                            CarGeneration carGeneration = new CarGeneration { Name = values[7], RefCarModelId = models[values[5]] };
                            carGenerations.Add(carGeneration);
                            generations.Add(carGeneration.Name, generationCounter++);
                        }

                        //// MOTORISIERUNG
                        CarTrim carTrim = new CarTrim { Name = values[1], RefCarGenerationId = generations[values[7]], Year = Convert.ToInt32(values[18]) };
                        carTrims.Add(carTrim);
                        trimCounter++;

                        if (!modelBodyMappings.Contains((bodyCounter - 1) + "_" + (modelCounter - 1)))
                        {
                            carModelBodyMappings.Add(new CarModelBodyMapping(modelCounter - 1, bodyCounter - 1));
                            modelBodyMappings.Add((bodyCounter - 1) + "_" + (modelCounter - 1));
                        }

                        //// MOTOR
                        CarEngine carEngine = new CarEngine { CarGear = (CarGear)(Convert.ToInt32(values[12])), EngineType = (EngineType)(Convert.ToInt32(values[14])), Power = Convert.ToInt32(values[17]), Volume = Convert.ToInt32(values[16]), RefCarTrimId = trimCounter - 1 };
                        carEngines.Add(carEngine);
                    }
                }

                DataContext.Instance.CarMakes.BulkInsert(carMakes);
                DataContext.Instance.CarModels.BulkInsert(carModels);
                DataContext.Instance.CarBodies.BulkInsert(carBodies);
                DataContext.Instance.CarGenerations.BulkInsert(carGenerations);
                DataContext.Instance.CarTrims.BulkInsert(carTrims);
                DataContext.Instance.CarEngines.BulkInsert(carEngines);
                DataContext.Instance.CarModelBodyMappings.BulkInsert(carModelBodyMappings);
            }
        }

        internal void SeedCostCenters()
        {
            var costCenterCategories = new List<CostCenterCategory>
            {
                new CostCenterCategory {Name = "Material"},
                new CostCenterCategory {Name = "Fertigung"},
                new CostCenterCategory {Name = "Verwaltung"},
                new CostCenterCategory {Name = "Vertrieb"}
            };

            DataContext.Instance.CostCenterCategories.Insert(costCenterCategories);

            var costCenters = new List<CostCenter>
            {
                new CostCenter {RefCostCenterCategoryId = 1, Identifier = "1", Name = "Beschaffung"},
                new CostCenter {RefCostCenterCategoryId = 1, Identifier = "2", Name = "Disposition"},
                new CostCenter {RefCostCenterCategoryId = 1, Identifier = "3", Name = "Lagerhaltung"},
                new CostCenter {RefCostCenterCategoryId = 2, Identifier = "4", Name = "Produktion / Montage"},
                new CostCenter {RefCostCenterCategoryId = 2, Identifier = "5", Name = "Qualitätssicherung"},
                new CostCenter {RefCostCenterCategoryId = 2, Identifier = "6", Name = "Arbeitsvorbereitung"},
                new CostCenter {RefCostCenterCategoryId = 2, Identifier = "7", Name = "Forschung und Entwicklung"},
                new CostCenter {RefCostCenterCategoryId = 3, Identifier = "8", Name = "Geschäftsführung"},
                new CostCenter {RefCostCenterCategoryId = 3, Identifier = "9", Name = "Buchhaltung / Finanzwesen / Controlling"},
                new CostCenter {RefCostCenterCategoryId = 3, Identifier = "10", Name = "Personalwesen"},
                new CostCenter {RefCostCenterCategoryId = 4, Identifier = "11", Name = "Marketing"},
                new CostCenter {RefCostCenterCategoryId = 4, Identifier = "12", Name = "Vertrieb"},
                new CostCenter {RefCostCenterCategoryId = 4, Identifier = "13", Name = "Fakturierung"},
                new CostCenter {RefCostCenterCategoryId = 4, Identifier = "14", Name = "Auftragswesen"}
            };

            foreach (var item in costCenters)
            {
                int id = DataContext.Instance.CostCenters.Insert(item);
                var costCenterBudget = new CostCenterBudget()
                {
                    RefCostCenterId = id,
                    Year = DateTime.Now.Year
                };
                DataContext.Instance.CostCenterBudgets.Insert(costCenterBudget);
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

            DataContext.Instance.CostAccountCategories.Insert(_CostAccounts);
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

            DataContext.Instance.CostAccountCategories.Insert(_CostAccounts);
        }

        public void ImportUserRights()
        {
            var rights = new List<UserRight>
            {
                new UserRight(Permission.AccessAccounting, "Buchhaltung", 0, "Erlaubt den Zugriff auf den Menüpunkt Buchhaltung"),
                new UserRight(Permission.AccessProjectManagement, "Projektmanagement", 0, "Erlaubt den Zugriff auf den Menüpunkt Projektmanagement"),
                new UserRight(Permission.AccessConfiguration, "Konfiguration", 0, "Erlaubt den Zugriff auf den Menüpunkt Konfiguration"),
                new UserRight(Permission.AccessWarehouseManagement, "Lagerverwaltung", 0, "Erlaubt den Zugriff auf den Menüpunkt Lagerverwaltung"),
                new UserRight(Permission.AccessProductManagement, "Produktverwaltung", 0, "Erlaubt den Zugriff auf den Menüpunkt Produktverwaltung"),
                //new UserRight(Permission.AccessPurchaseManagement, "Bestellverwaltung", 0, "Erlaubt den Zugriff auf den Menüpunkt Bestellverwaltung"),
                new UserRight(Permission.AccessSalesManagement, "Verkaufsverwaltung", 0, "Erlaubt den Zugriff auf den Menüpunkt Verkaufsverwaltung"),

                new UserRight(Permission.AccessBookings, "Buchungen", (int) Permission.AccessAccounting, "Erlaubt den Zugriff auf den Menüpunkt Buchungen"),
                new UserRight(Permission.AccessBookingHistory, "Buchungshistorie", (int) Permission.AccessAccounting, "Erlaubt den Zugriff auf den Menüpunkt Buchungshistorie"),
                new UserRight(Permission.AccessCreditorDebitors, "Kreditoren und Debitoren", (int) Permission.AccessAccounting, "Erlaubt den Zugriff auf den Menüpunkt Kreditoren und Debitoren"),
                new UserRight(Permission.AccessTaxTypes, "Steuersätze", (int) Permission.AccessAccounting, "Erlaubt den Zugriff auf den Menüpunkt Steuersätze"),
                new UserRight(Permission.AccessCostAccounts, "Kontenrahmen", (int) Permission.AccessAccounting, "Erlaubt den Zugriff auf den Menüpunkt Kontenrahmen"),
                new UserRight(Permission.AccessPaymentCondiditions, "Zahlungsbedingungen", (int) Permission.AccessAccounting, "Erlaubt den Zugriff auf den Menüpunkt Zahlungsbedingungen"),

                new UserRight(Permission.AccessCostCenters, "Kostenstellen", (int) Permission.AccessProjectManagement, "Erlaubt den Zugriff auf den Menüpunkt Kostenstellen"),
                new UserRight(Permission.AccessCostCenterCategories, "Kostenstellenkategorien", (int) Permission.AccessProjectManagement, "Erlaubt den Zugriff auf den Menüpunkt Kostenstellenkategorien"),
                new UserRight(Permission.AccessEmployees, "Mitarbeiter", (int) Permission.AccessProjectManagement, "Erlaubt den Zugriff auf den Menüpunkt Mitarbeiter"),
                new UserRight(Permission.AccessProjects, "Projekte", (int) Permission.AccessProjectManagement, "Erlaubt den Zugriff auf den Menüpunkt Projekte"),
                new UserRight(Permission.AccessProjectWorkingTimes, "Zeiterfassungen", (int) Permission.AccessProjectManagement, "Erlaubt den Zugriff auf den Menüpunkt Zeiterfassungen"),

                new UserRight(Permission.AccessMail, "Mailkonfiguration", (int) Permission.AccessConfiguration, "Erlaubt den Zugriff auf den Menüpunkt Mailkonfiguration"),
                new UserRight(Permission.AccessUsers, "Benutzer", (int) Permission.AccessConfiguration, "Erlaubt den Zugriff auf den Menüpunkt Benutzer"),
                new UserRight(Permission.AccessMyCompanies, "Eigene Firma", (int) Permission.AccessConfiguration, "Erlaubt den Zugriff auf den Menüpunkt Eigene Firma"),

                new UserRight(Permission.AccessWarehouses, "Lager", (int) Permission.AccessWarehouseManagement, "Erlaubt den Zugriff auf den Menüpunkt Lager"),
                new UserRight(Permission.AccessWarehouseSave, "Lager speichern", (int) Permission.AccessWarehouses, "Erlaubt Änderungen und neue Lager zu speichern"),
                new UserRight(Permission.AccessWarehouseDelete, "Lager löschen", (int) Permission.AccessWarehouses, "Erlaubt Lager zu löschen."),
                new UserRight(Permission.AccessStockyards, "Lagerplätze", (int) Permission.AccessWarehouseManagement, "Erlaubt den Zugriff auf den Menüpunkt Lagerplätze"),
                new UserRight(Permission.AccessStocking, "Ein- und Auslagerung", (int) Permission.AccessWarehouseManagement, "Erlaubt den Zugriff auf den Menüpunkt Ein- und Auslagerung"),
                new UserRight(Permission.AccessStockingStore, "Einlagerung", (int) Permission.AccessStocking, "Erlaubt den Zugriff auf den Menüpunkt Einlagerung"),
                new UserRight(Permission.AccessStockingTakeOut, "Auslagerung", (int) Permission.AccessStocking, "Erlaubt den Zugriff auf den Menüpunkt Auslagerung"),

                new UserRight(Permission.AccessProducts, "Produkte", (int) Permission.AccessProductManagement, "Erlaubt den Zugriff auf den Menüpunkt Produkte"),
                new UserRight(Permission.AccessProductCategories, "Produktkategorien", (int) Permission.AccessProducts, "Erlaubt den Zugriff auf den Menüpunkt Produktkategorien"),

                //new UserRight(Permission.AccessPurchaseOrders, "Bestellungen", (int) Permission.AccessPurchaseManagement, "Erlaubt den Zugriff auf den Menüpunkt Bestellungen"),
                //new UserRight(Permission.AccessPurchaseTypes, "Bestellungsart", (int) Permission.AccessPurchaseOrders, "Erlaubt den Zugriff auf den Menüpunkt Bestellungsart"),
                //new UserRight(Permission.AccessBills, "Rechnungen", (int) Permission.AccessPurchaseManagement, "Erlaubt den Zugriff auf den Menüpunkt Rechnungen"),
                //new UserRight(Permission.AccessBillTypes, "Rechnungsarten", (int) Permission.AccessBills, "Erlaubt den Zugriff auf den Menüpunkt Rechnungsart"),

                new UserRight(Permission.AccessSalesOrders, "Verkäufe", (int) Permission.AccessSalesManagement, "Erlaubt den Zugriff auf den Menüpunkt Verkäufe"),
                new UserRight(Permission.AccessSalesTypes, "Verkaufsarten", (int) Permission.AccessSalesOrders, "Erlaubt den Zugriff auf den Menüpunkt Verkaufsart"),
                new UserRight(Permission.AccessInvoices, "Rechnung", (int) Permission.AccessSalesManagement, "Erlaubt den Zugriff auf den Menüpunkt Rechnung"),
                new UserRight(Permission.AccessInvoiceTypes, "Rechnungarten", (int) Permission.AccessInvoices, "Erlaubt den Zugriff auf den Menüpunkt Rechnungart"),
                new UserRight(Permission.AccessShipmentTypes, "Versand", (int) Permission.AccessSalesManagement, "Erlaubt den Zugriff auf den Menüpunkt Versand"),
                new UserRight(Permission.AccessShipments, "Versandtyp", (int) Permission.AccessSalesManagement, "Erlaubt den Zugriff auf den Menüpunkt Versandtyp")
            };

            foreach (var item in rights)
            {
                item.UserRightId = DataContext.Instance.UserRights.Insert(item);
                DataContext.Instance.UserRightUserMappings.Insert(new UserRightUserMapping
                { RefUserId = 1, RefUserRightId = item.UserRightId, IsGranted = true });
            }
        }

        public void SeedTypes()
        {
            DataContext.Instance.InvoiceTypes.Insert(new InvoiceType { Name = "Allgemein" });
            DataContext.Instance.SalesTypes.Insert(new SalesType { Name = "Allgemein" });
            DataContext.Instance.ShipmentTypes.Insert(new ShipmentType { Name = "Allgemein" });
            DataContext.Instance.ProductCategories.Insert(new ProductCategory { Name = "Allgemein" });
        }

        /// <summary>
        ///     Seeds the table with initial data
        /// </summary>
        public void SeedTaxTypes()
        {
            var taxTypes = new List<TaxType>
            {
                new TaxType
                {
                    DescriptionShort = "Keine", Description = "Keine", AmountOfTax = 0, TaxCategory = TaxCategory.None
                },
                new TaxType
                {
                    DescriptionShort = "Bau 7%", Description = "Bau mit 7% USt/VSt", AmountOfTax = 7,
                    TaxCategory = TaxCategory.thirteenB, RefCostAccount = GetIdOfCostAccount(1785)
                },
                new TaxType
                {
                    DescriptionShort = "I.g.E 7% USt/VSt", Description = "I.g.E 7% USt/VSt", AmountOfTax = 7,
                    TaxCategory = TaxCategory.igE, RefCostAccount = GetIdOfCostAccount(1773)
                },
                new TaxType
                {
                    DescriptionShort = "I.g.E 16% USt/VSt", Description = "I.g.E 16% USt/VSt", AmountOfTax = 16,
                    TaxCategory = TaxCategory.igE, RefCostAccount = GetIdOfCostAccount(1774)
                },
                new TaxType
                {
                    DescriptionShort = "I.g.E 19% USt/VSt", Description = "I.g.E 19% USt/VSt", AmountOfTax = 19,
                    TaxCategory = TaxCategory.igE, RefCostAccount = GetIdOfCostAccount(1772)
                },
                new TaxType
                {
                    DescriptionShort = "I.g.E Neufahrzeug", Description = "I.g.E Neufahrzeuge 19% USt/VSt",
                    AmountOfTax = 19, TaxCategory = TaxCategory.igE, RefCostAccount = GetIdOfCostAccount(1784)
                },
                new TaxType
                {
                    DescriptionShort = "Kfz 19% VSt. 50%", Description = "Kfz 19% Vorsteuer. 50%", AmountOfTax = 19,
                    TaxCategory = TaxCategory.fiftyPercent, RefCostAccount = GetIdOfCostAccount(1570)
                },
                new TaxType
                {
                    DescriptionShort = "Kfz VSt. 50%", Description = "Kfz Vorsteuer. 50%", AmountOfTax = 16,
                    TaxCategory = TaxCategory.fiftyPercent, RefCostAccount = GetIdOfCostAccount(1570)
                },
                new TaxType
                {
                    DescriptionShort = "USt. 15%", Description = "Umsatzsteuer 15%", AmountOfTax = 15,
                    TaxCategory = TaxCategory.Netto, RefCostAccount = GetIdOfCostAccount(1770)
                },
                new TaxType
                {
                    DescriptionShort = "USt. 16%", Description = "Umsatzsteuer 16%", AmountOfTax = 16,
                    TaxCategory = TaxCategory.Netto, RefCostAccount = GetIdOfCostAccount(1775)
                },
                new TaxType
                {
                    DescriptionShort = "USt. 19%", Description = "Umsatzsteuer 19%", AmountOfTax = 19,
                    TaxCategory = TaxCategory.Netto, RefCostAccount = GetIdOfCostAccount(1776)
                },
                new TaxType
                {
                    DescriptionShort = "USt. 7%", Description = "Umsatzsteuer 7%", AmountOfTax = 7,
                    TaxCategory = TaxCategory.Netto, RefCostAccount = GetIdOfCostAccount(1771)
                },
                new TaxType
                {
                    DescriptionShort = "USt/VSt 19%",
                    Description = "Reverse Charge (Steuerschuld Leistungsempf.) 19% USt/VSt", AmountOfTax = 19,
                    TaxCategory = TaxCategory.thirteenB, RefCostAccount = GetIdOfCostAccount(1787)
                },
                new TaxType
                {
                    DescriptionShort = "USt/VSt 7%",
                    Description = "Reverse Charge (Steuerschuld Leistungsempf.) 7% USt/VSt", AmountOfTax = 7,
                    TaxCategory = TaxCategory.thirteenB, RefCostAccount = GetIdOfCostAccount(1785)
                },
                new TaxType
                {
                    DescriptionShort = "VSt. 15%", Description = "Vorsteuer 15%", AmountOfTax = 15,
                    TaxCategory = TaxCategory.Netto, RefCostAccount = GetIdOfCostAccount(1771)
                },
                new TaxType
                {
                    DescriptionShort = "VSt. 16%", Description = "Vorsteuer 16%", AmountOfTax = 16,
                    TaxCategory = TaxCategory.Netto, RefCostAccount = GetIdOfCostAccount(1575)
                },
                new TaxType
                {
                    DescriptionShort = "VSt. 19%", Description = "Vorsteuer 19%", AmountOfTax = 19,
                    TaxCategory = TaxCategory.Netto, RefCostAccount = GetIdOfCostAccount(1576)
                },
                new TaxType
                {
                    DescriptionShort = "VSt. 7%", Description = "Vorsteuer 7%", AmountOfTax = 7,
                    TaxCategory = TaxCategory.Netto, RefCostAccount = GetIdOfCostAccount(1571)
                }
            };

            DataContext.Instance.TaxTypes.Insert(taxTypes);
        }

        public void SeedCompany()
        {
            var client = new Client
            {
                Name = "Max Mustermann GmbH",
                Street = "Beispielstrasse 1",
                City = "Musterhausen",
                Postcode = 12345
            };

            client.ClientId = DataContext.Instance.Clients.Insert(client);

            var company = new Company
            {
                CEO = "Sven Fuhrmann",
                ContactPerson = "Sven Fuhrmann",
                RefClientId = client.ClientId
            };

            DataContext.Instance.Companies.Insert(company);
        }

        public void SeedHealthInsurance()
        {
            var healthInsurance = new HealthInsurance { Name = "Keine" };

            DataContext.Instance.HealthInsurances.Insert(healthInsurance);
        }

        public void SeedPaymentCondition()
        {
            var paymentCondition = new PaymentCondition { Name = "Keine", Percentage = 0, TimeValue = 0, PayType = PayType.ThisMonth };

            DataContext.Instance.PaymentConditions.Insert(paymentCondition);
        }

        private int GetIdOfCostAccount(int AccountNumber)
        {
            return DataContext.Instance.CostAccounts.GetByAccountNumber(AccountNumber);
        }
    }
}