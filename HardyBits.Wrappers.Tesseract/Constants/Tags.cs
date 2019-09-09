namespace HardyBits.Wrappers.Tesseract.Constants
{
  public static class Tags
  {
    public const string XhtmlBeginTag =
      "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n"
      + "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\"\n"
      + "    \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\n"
      + "<html xmlns=\"http://www.w3.org/1999/xhtml\" xml:lang=\"en\" "
      + "lang=\"en\">\n <head>\n  <title></title>\n"
      + "<meta http-equiv=\"Content-Type\" content=\"text/html;"
      + "charset=utf-8\" />\n"
      + "  <meta name='ocr-system' content='tesseract' />\n"
      + "  <meta name='ocr-capabilities' content='ocr_page ocr_carea ocr_par"
      + " ocr_line ocrx_word"
      + "'/>\n"
      + "</head>\n<body>\n";

    public const string XhtmlEndTag = " </body>\n</html>\n";
  }
}
