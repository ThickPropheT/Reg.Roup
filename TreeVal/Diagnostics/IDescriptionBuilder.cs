namespace TreeVal.Diagnostics;

public interface IDescriptionBuilder
{
    void Indented(Action body, int count = 1);
    
    void EmitNewline(int count = 1);

    void Emit(string text);
    void EmitLine(string text);
    
    void EmitOpenBlock();
    void EmitCloseBlock();

    void EmitHeader(string message);
    void EmitFooter();
    
    void EmitBlock(Action body);
    void EmitBlock(string heading, Action body);
    void EmitError(Exception error);

    string ToString();
}
