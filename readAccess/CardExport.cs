namespace readAccess
{
    public class CardExport
    {
        // توجه: نام دقیقاً مطابق دیتابیس
        [ColumnName("id")]
        public int id { get; set; }  // حتماً با حروف کوچک

        [ColumnName("BranchCode")]
        public string? BranchCode { get; set; }

        [ColumnName("AccCnt")]
        public string? AccCnt { get; set; }

        [ColumnName("PAN1")]
        public string? PAN1 { get; set; }

        [ColumnName("PAN2")]
        public string? PAN2 { get; set; }

        [ColumnName("PAN3")]
        public string? PAN3 { get; set; }

        [ColumnName("PAN4")]
        public string? PAN4 { get; set; }

        [ColumnName("FName")]
        public string? FName { get; set; }

        [ColumnName("IMD")]
        public string? IMD { get; set; }

        [ColumnName("Track2")]
        public string? Track2 { get; set; }

        [ColumnName("Exp_Year")]
        public string? Exp_Year { get; set; }

        [ColumnName("Exp_Month")]
        public string? Exp_Month { get; set; }

        [ColumnName("CVV")]
        public string? CVV { get; set; }

        [ColumnName("CVV2")]
        public string? CVV2 { get; set; }

        [ColumnName("CardType")]
        public string? CardType { get; set; }

        [ColumnName("Sahmiehcode")]
        public string? Sahmiehcode { get; set; }

        [ColumnName("Customer_NTN")]
        public string? Customer_NTN { get; set; }

        [ColumnName("GiftCardText")]
        public string? GiftCardText { get; set; }

        [ColumnName("GiftCardAmount")]
        public string? GiftCardAmount { get; set; }

        [ColumnName("Sheba")]
        public string? Sheba { get; set; }

        [ColumnName("ReferenceNo")]
        public string? ReferenceNo { get; set; }

    }
}
