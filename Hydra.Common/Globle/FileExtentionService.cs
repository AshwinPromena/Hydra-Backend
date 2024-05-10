namespace Hydra.Common.Globle
{
    public class FileExtentionService
    {
        public string GetExtension(string subString)
        {
            string extension = subString.ToUpper() switch
            {
                ExtententionConstants.PngSubString => ExtententionConstants.PngExt,
                ExtententionConstants.JpgSubString => ExtententionConstants.JpgExt,
                ExtententionConstants.mp4SubString => ExtententionConstants.mp4Ext,
                ExtententionConstants.pdfSubString => ExtententionConstants.pdfExt,
                ExtententionConstants.icoSubString => ExtententionConstants.icoExt,
                ExtententionConstants.rarSubString => ExtententionConstants.rarExt,
                ExtententionConstants.rtfSubString => ExtententionConstants.rtfExt,
                ExtententionConstants.txtSubString => ExtententionConstants.txtExt,
                ExtententionConstants.srtSubStringa or ExtententionConstants.srtSubStringb => ExtententionConstants.srtExt,
                _ => string.Empty,
            };
            return extension;
        }

        public class ExtententionConstants
        {
            public const string PDF = "pdf";
            public const string JPG = "jpg";
            public const string PNG = "png";
            public const string MP4 = "mp4";
            public const string PngExt = ".png";
            public const string PngSubString = "IVBOR";
            public const string JpgExt = ".jpg";
            public const string JpgSubString = "/9J/4";
            public const string mp4Ext = ".mp4";
            public const string mp4SubString = "AAAAF";
            public const string pdfExt = ".pdf";
            public const string pdfSubString = "JVBER";
            public const string icoExt = ".ico";
            public const string icoSubString = "AAABA";
            public const string rarExt = ".rar";
            public const string rarSubString = "UMFYI";
            public const string rtfExt = ".rtf";
            public const string rtfSubString = "E1XYD";
            public const string txtExt = ".txt";
            public const string txtSubString = "U1PKC";
            public const string srtExt = ".srt";
            public const string srtSubStringa = "MQOWM";
            public const string srtSubStringb = "77U/M";
        }
    }
}
