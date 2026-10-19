using System.Text.RegularExpressions;

namespace readAccess
{
    public class CardExport
    {

        [ColumnName("ID", Order = 0)]
        public int ID { get; set; }

        [ColumnName("PIN", Order = 1)]
        public string? PIN { get; set; }

        [ColumnName("PAN_PART1", Order = 2)]
        public string? PAN_PART1 { get; set; }

        [ColumnName("PAN_PART2", Order = 3)]
        public string? PAN_PART2 { get; set; }

        [ColumnName("PAN_PART3", Order = 4)]
        public string? PAN_PART3 { get; set; }

        [ColumnName("PAN_PART4", Order = 5)]
        public string? PAN_PART4 { get; set; }

        [ColumnName("PAN_PART5", Order = 6)]
        public string? PAN_PART5 { get; set; }

        [ColumnName("TRACK1", Order = 7)]
        public string? Track1 { get; set; }

        [ColumnName("TRACK2", Order = 8)]
        public string? Track2 { get; set; }

        [ColumnName("TRACK3", Order = 9)]
        public string? Track3 { get; set; }

        [ColumnName("NAME_FR", Order = 10)]
        public string? NAME_FR { get; set; }

        [ColumnName("NAME_EN", Order = 11)]
        public string? NAME_EN { get; set; }

        [ColumnName("ISUDATE", Order = 12)]
        public string? ISUDATE { get; set; }

        [ColumnName("EXPDATE", Order = 13)]
        public string? EXPDATE { get; set; }

        [ColumnName("CIFNO", Order = 14)]
        public string? CIFNO { get; set; }

        [ColumnName("ISUDATE_ANSAR", Order = 15)]
        public string? ISUDATE_ANSAR { get; set; }

        [ColumnName("BRANCHCODE", Order = 16)]
        public string? BRANCHCODE { get; set; }

        [ColumnName("RESULT", Order = 17)]
        public string? RESULT { get; set; }

        [ColumnName("REQTYPE", Order = 18)]
        public string? REQTYPE { get; set; }


        [ColumnName("NATIONALID", Order = 19)]
        public string? NATIONALID { get; set; }

        [ColumnName("PAN", Order = 20)]
        public string? PAN { get; set; }

        [ColumnName("CRN", Order = 21)]
        public string? CRN { get; set; }

        [ColumnName("MILADIEXPDATE", Order = 22)]
        public string? MILADIEXPDATE { get; set; }

        [ColumnName("CREDIT_AMOUNT", Order = 23)]
        public string? CREDIT_AMOUNT { get; set; }

        [ColumnName("MAIN_ACOUNT", Order = 24)]
        public string? MAIN_ACOUNT { get; set; }

        [ColumnName("CREDIT_MACNA_CODE", Order = 25)]
        public string? CREDIT_MACNA_CODE { get; set; }

        [ColumnName("CREDIT_CYCLE_BEG", Order = 26)]
        public string? CREDIT_CYCLE_BEG { get; set; }

        [ColumnName("EPAY_PIN", Order = 27)]
        public string? EPAY_PIN { get; set; }

        [ColumnName("PRINT_EXPRESSION", Order = 28)]
        public string? PRINT_EXPRESSION { get; set; }

        [ColumnName("PREPAID_AMOUNT", Order = 29)]
        public string? PREPAID_AMOUNT { get; set; }

        [ColumnName("HOLDERID", Order = 30)]
        public string? HOLDERID { get; set; }

        [ColumnName("CardTitle", Order = 31)]
        public string? CardTitle { get; set; }

        [ColumnName("ForeignCardTitle", Order = 32)]
        public string? ForeignCardTitle { get; set; }

        [ColumnName("IssuerBranchCode", Order = 33)]
        public string? IssuerBranchCode { get; set; }

        [ColumnName("PREPAID_AMOUNT_EN", Order = 34)]
        public string? PREPAID_AMOUNT_EN { get; set; }

        [ColumnName("PRSNLCODE", Order = 35)]
        public string? PRSNLCODE { get; set; }

        [ColumnName("EXPDATE_FourDigit", Order = 36)]
        public string? EXPDATE_FourDigit { get; set; }

        [ColumnName("GregorianExpiryDate_FullFormat", Order = 37)]
        public string? GregorianExpiryDate_FullFormat { get; set; }

        [ColumnName("GregorianExpiryDate_SixDigit", Order = 38)]
        public string? GregorianExpiryDate_SixDigit { get; set; }

        [ColumnName("GregorianIssueDate_FullFormat", Order = 39)]
        public string? GregorianIssueDate_FullFormat { get; set; }

        [ColumnName("GregorianIssueDate_SixDigit", Order = 40)]
        public string? GregorianIssueDate_SixDigit { get; set; }

        [ColumnName("GregorianIssueDate_FourDigit", Order = 41)]
        public string? GregorianIssueDate_FourDigit { get; set; }

        [ColumnName("Card_Owner_Name", Order = 42)]
        public string? Card_Owner_Name { get; set; }

        [ColumnName("Requester_Branch_Name", Order = 43)]
        public string? Requester_Branch_Name { get; set; }

        [ColumnName("Iban", Order = 44)]
        public string? Iban { get; set; }

        [ColumnName("Address", Order = 45)]
        public string? Address { get; set; }

        [ColumnName("PostalCode", Order = 46)]
        public string? PostalCode { get; set; }

        [ColumnName("CellPhone", Order = 47)]
        public string? CellPhone { get; set; }



    }
}
