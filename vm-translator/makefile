all: VMTranslator

VMTranslator: VMTranslator.cs Parser.cs CodeWriter.cs Logger.cs
	dotnet publish -o .

clean:
	rm ./VMTranslator
	rm -r bin/
	rm -r obj/
