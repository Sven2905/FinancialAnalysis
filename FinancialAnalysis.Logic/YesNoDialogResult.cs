namespace FinancialAnalysis.Logic
{
    public class YesNoDialogResult
    {
        public YesNoDialogResult()
        {
        }

        public YesNoDialogResult(bool DialogResult)
        {
            Result = DialogResult;
        }

        public bool Result { get; set; }
    }
}