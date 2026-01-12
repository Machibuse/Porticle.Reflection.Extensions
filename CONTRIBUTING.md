# Contributing to Porticle.Reflection.Extensions

First off, thank you for considering contributing to Porticle.Reflection.Extensions! It's people like you that make this library better for everyone.

## Code of Conduct

This project and everyone participating in it is governed by our commitment to providing a welcoming and inspiring community for all. Please be respectful and constructive in all interactions.

## How Can I Contribute?

### Reporting Bugs

Before creating bug reports, please check the existing issues to see if the problem has already been reported. When you are creating a bug report, please include as many details as possible:

- **Use a clear and descriptive title** for the issue
- **Describe the exact steps which reproduce the problem** with as much detail as possible
- **Provide specific examples** to demonstrate the steps
- **Describe the behavior you observed** after following the steps
- **Explain which behavior you expected to see instead** and why
- **Include code samples** that demonstrate the issue
- **Specify the .NET version** you're using (.NET 8.0, 9.0, or 10.0)

### Suggesting Enhancements

Enhancement suggestions are tracked as GitHub issues. When creating an enhancement suggestion, please include:

- **Use a clear and descriptive title** for the issue
- **Provide a step-by-step description** of the suggested enhancement
- **Provide specific examples** to demonstrate the enhancement
- **Describe the current behavior** and **explain the behavior you would like to see instead**
- **Explain why this enhancement would be useful** to most users

### Pull Requests

- Fill in the required template
- Follow the C# coding style defined in the `.editorconfig`
- Include thoughtfully-worded, well-structured tests
- Document new code with XML documentation comments
- End all files with a newline
- Avoid platform-dependent code

## Development Setup

### Prerequisites

- .NET SDK 8.0, 9.0, or 10.0
- Git
- An IDE that supports EditorConfig (Visual Studio, Visual Studio Code, Rider, etc.)

### Getting Started

1. **Fork and clone the repository**
   ```bash
   git clone https://github.com/YOUR-USERNAME/Porticle.Reflection.Extensions.git
   cd Porticle.Reflection.Extensions
   ```

2. **Navigate to the Source directory**
   ```bash
   cd Source
   ```

3. **Restore dependencies**
   ```bash
   dotnet restore
   ```

4. **Build the project**
   ```bash
   dotnet build
   ```

5. **Run the tests**
   ```bash
   dotnet test
   ```

### Project Structure

```
Porticle.Reflection.Extensions/
├── Source/
│   ├── Porticle.Reflection.Extensions/
│   │   ├── ReflectionExtensions.cs          # Main implementation
│   │   └── Porticle.Reflection.Extensions.csproj
│   ├── Porticle.Reflection.Extensions.Tests/
│   │   ├── TypeExtensionsTests.cs           # Tests for Type overload
│   │   ├── PropertyInfoExtensionsTests.cs   # Tests for PropertyInfo overload
│   │   ├── ParameterInfoExtensionsTests.cs  # Tests for ParameterInfo overload
│   │   ├── FieldInfoExtensionsTests.cs      # Tests for FieldInfo overload
│   │   ├── EventInfoExtensionsTests.cs      # Tests for EventInfo overload
│   │   ├── TestFixtures.cs                  # Test helper classes
│   │   └── Porticle.Reflection.Extensions.Tests.csproj
│   └── Porticle.Reflection.sln
├── .editorconfig                            # Code style rules
├── .gitignore
├── CHANGELOG.md                             # Version history
├── CONTRIBUTING.md                          # This file
├── LICENSE
└── README.md
```

## Coding Guidelines

### Code Style

This project uses an `.editorconfig` file to maintain consistent code style. Key conventions:

- **Indentation**: 4 spaces for C# files
- **Line endings**: CRLF (Windows-style)
- **Encoding**: UTF-8
- **Naming**: PascalCase for public members, camelCase for parameters
- **Braces**: Always use braces for control flow statements
- **var**: Use `var` when type is apparent, otherwise explicit types
- **Nullability**: Nullable reference types are enabled - use `?` appropriately

### Documentation

- All public APIs must have XML documentation comments
- Include `<summary>`, `<param>`, `<returns>`, and `<exception>` tags where appropriate
- Provide code examples in `<example>` tags for complex methods
- Use `<remarks>` for additional context or warnings

### Testing

- Write tests for all new functionality
- Use xUnit with Theory/InlineData for parameterized tests
- Organize tests into logical regions using `#region`
- Test files should mirror the structure of the implementation
- Aim for high code coverage (current: 125+ tests)
- Test edge cases and error conditions

### Commit Messages

- Use clear and meaningful commit messages
- Start with a verb in imperative mood (e.g., "Add", "Fix", "Update")
- Keep the first line under 72 characters
- Provide additional details in the body if necessary

Example:
```
Add support for pointer types in type conversion

- Implement handling for Type.IsPointer
- Add tests for pointer type scenarios
- Update documentation with pointer examples
```

## Testing Your Changes

Before submitting a pull request, ensure:

1. **All tests pass**
   ```bash
   dotnet test
   ```

2. **Code builds without warnings**
   ```bash
   dotnet build --configuration Release
   ```

3. **Run code coverage** (optional but recommended)
   ```bash
   dotnet test --collect:"XPlat Code Coverage"
   ```

4. **Test all target frameworks** if possible
   ```bash
   dotnet test --framework net8.0
   dotnet test --framework net9.0
   dotnet test --framework net10.0
   ```

## Submitting Changes

1. **Create a new branch** for your changes
   ```bash
   git checkout -b feature/my-new-feature
   ```

2. **Make your changes** following the coding guidelines

3. **Add tests** for your changes

4. **Update documentation** (README.md, XML comments, etc.)

5. **Update CHANGELOG.md** under the "Unreleased" section

6. **Commit your changes** with clear commit messages

7. **Push to your fork**
   ```bash
   git push origin feature/my-new-feature
   ```

8. **Open a Pull Request** against the `main` branch

### Pull Request Checklist

- [ ] Code follows the project's style guidelines
- [ ] All tests pass
- [ ] New tests added for new functionality
- [ ] XML documentation added/updated
- [ ] CHANGELOG.md updated
- [ ] No breaking changes (or clearly documented if unavoidable)
- [ ] README.md updated if needed

## Questions?

If you have questions, feel free to:
- Open an issue with the "question" label
- Check existing issues and discussions

Thank you for contributing! 🎉
