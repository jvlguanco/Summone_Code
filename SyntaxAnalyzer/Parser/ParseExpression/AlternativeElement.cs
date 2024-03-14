namespace Core.Library.RE;

internal class AlternativeElement : Element {
    private Element elem1;

    private Element elem2;

    public AlternativeElement(Element first, Element second) {
        elem1 = first;
        elem2 = second;
    }

    public override object Clone() {
        return new AlternativeElement(elem1, elem2);
    }

    public override int Match(Matcher m,
                              ReaderBuffer buffer,
                              int start,
                              int skip) {

        int  length = 0;
        int  length1 = -1;
        int  length2 = -1;
        int  skip1 = 0;
        int  skip2 = 0;

        while (length >= 0 && skip1 + skip2 <= skip) {
            length1 = elem1.Match(m, buffer, start, skip1);
            length2 = elem2.Match(m, buffer, start, skip2);
            if (length1 >= length2) {
                length = length1;
                skip1++;
            } else {
                length = length2;
                skip2++;
            }
        }
        return length;
    }

    public override void PrintTo(TextWriter output, string indent) {
        output.WriteLine(indent + "Alternative 1");
        elem1.PrintTo(output, indent + "  ");
        output.WriteLine(indent + "Alternative 2");
        elem2.PrintTo(output, indent + "  ");
    }
}