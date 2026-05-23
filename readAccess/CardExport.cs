namespace readAccess
{
    public class CardExport
    {
        // توجه: نام دقیقاً مطابق دیتابیس
        public int id { get; set; }  // حتماً با حروف کوچک

        [System.ComponentModel.DataAnnotations.Schema.Column("BranchCode")]
        public string? BranchCode { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.Column("AccCnt")]
        public string? AccCnt { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.Column("PAN1")]
        public string? PAN1 { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.Column("PAN2")]
        public string? PAN2 { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.Column("PAN3")]
        public string? PAN3 { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.Column("PAN4")]
        public string? PAN4 { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.Column("FName")]
        public string? FName { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.Column("IMD")]
        public string? IMD { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.Column("Track2")]
        public string? Track2 { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.Column("Exp_Year")]
        public string? Exp_Year { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.Column("Exp_Month")]
        public string? Exp_Month { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.Column("CVV")]
        public string? CVV { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.Column("CVV2")]
        public string? CVV2 { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.Column("CardType")]
        public string? CardType { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.Column("Sahmiehcode")]
        public string? Sahmiehcode { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.Column("Customer_NTN")]
        public string? Customer_NTN { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.Column("GiftCardText")]
        public string? GiftCardText { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.Column("GiftCardAmount")]
        public string? GiftCardAmount { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.Column("Sheba")]
        public string? Sheba { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.Column("ReferenceNo")]
        public string? ReferenceNo { get; set; }

    }
}
