using ExcelWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace ExcelWebAPI.Managers
{
    public class DocumentManager : IDocumentManager
    {
        private readonly ExcelWebApiContext _context;

        public DocumentManager(ExcelWebApiContext context)
        {
            _context = context;
        }

        public async Task<Sheet> CreateSheetAsync(string sheetId)
        {
            Sheet newSheet = new()
            {
                Id = sheetId,
                Cells = new()
            };
            await _context.Sheets.AddAsync(newSheet);
            return newSheet;
        }

        public async Task<Cell> CreateCellAsync(string sheetId, string cellId)
        {
            Cell newCell = new()
            {
                Id = cellId,
                SheetId = sheetId
            };
            await _context.Cells.AddAsync(newCell);
            return newCell;
        }

        public async Task<Cell> SetSheetCellAsync(string sheetId, string cellId, string value)
        {
            Sheet sheet = await GetSheetAsync(sheetId)
                           ?? await CreateSheetAsync(sheetId);

            Cell cell = await GetSheetCellAsync(sheet.Id, cellId)
                         ?? await CreateCellAsync(sheet.Id, cellId);

            cell.Value = value;

            await _context.SaveChangesAsync();

            return cell;
        }

        public async Task<Sheet?> GetSheetAsync(string sheetId)
        {
            return await _context.Sheets.Include(x => x.Cells).FirstOrDefaultAsync(x => x.Id == sheetId) ?? null;
        }

        public async Task<Cell?> GetSheetCellAsync(string sheetId, string cellId)
        {
            Sheet? sheet = await GetSheetAsync(sheetId);
            if (sheet == null)
            {
                return null;
            }
            return await _context.Cells.FirstOrDefaultAsync(x => x.Id == cellId) ?? null;
        }

        public async Task<string> GetResult(string sheetId, string cellId, string cellValue)
        {
            if (cellValue[0] != '=')
            {
                return cellValue;
            }

            while (cellValue.Contains('(') && cellValue.Contains(')'))
            {
                var index1 = cellValue.LastIndexOf('(');
                int index2 = 0;
                for (int i = index1+1; i < cellValue.Length; i++)
                {
                    char currentChar = cellValue[i];
                    if (currentChar == ')')
                    {
                        index2 = i;
                        break;
                    }
                }
                if(index2==0)
                {
                    return "ERROR";
                }
                string substring = '=' + cellValue[(index1 + 1)..index2];
                string result = await GetResult(sheetId, cellId, substring);

                substring = cellValue[index1..(index2 + 1)];
                cellValue = cellValue.Replace(substring, result);
            }

            if (cellValue.Contains('(') && !cellValue.Contains(')')
                || !cellValue.Contains('(') && cellValue.Contains(')'))
            {
                return "ERROR";
            }


            List<string> operators = new();
            SplitIntoOperators(cellValue, ref operators);
            if (operators.Contains(cellId))
            {
                return "ERROR";
            }

            List<string>? values = new();
            await GetOperatorsValues(sheetId, values, operators);
            if (values.Count == 0)
            {
                return "ERROR";
            }
            if (values.Count == 1)
            {
                return values[0];
            }

            List<char> operations = new();
            SplitIntoOperations(cellValue, ref operations);

            string res = "";
            char op = ' ';
            while (operations.Count > 0)
            {
                int index = 0;
                if (operations.Contains('*'))
                {
                    op = '*';
                    index = operations.IndexOf(op);
                }
                else if (operations.Contains('/'))
                {
                    op = '/';
                    index = operations.IndexOf(op);
                }
                else if (operations.Contains('+'))
                {
                    op = '+';
                    index = operations.IndexOf(op);
                }
                else if (operations.Contains('-'))
                {
                    op = '-';
                    index = operations.IndexOf(op);
                }
                res = Calculate(values[index], values[index + 1], op);
                operations.RemoveAt(index);
                values[index] = res;
                values.RemoveAt(index + 1);
            }
            return res;
        }
        private void SplitIntoOperators(string cellValue, ref List<string> operators)
        {
            operators = cellValue.Split('=', '+', '-', '*', '/').ToList();
            operators = operators.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
        }
        private void SplitIntoOperations(string cellValue, ref List<char> operations)
        {
            char[] operators = { '+', '-', '*', '/' };

            foreach (char c in cellValue)
            {
                if (Array.IndexOf(operators, c) != -1)
                {
                    operations.Add(c);
                }
            }
        }
        private async Task GetOperatorsValues(string sheetId, List<string> values, List<string> operators)
        {
            foreach (var item in operators)
            {
                bool variable = false;
                byte[] asciiBytes = Encoding.ASCII.GetBytes(item);
                foreach (var item1 in asciiBytes)
                {
                    if (item1 > 57)
                    {
                        variable = true;
                        break;
                    }
                }
                if (variable)
                {
                    Cell? cell = await GetSheetCellAsync(sheetId, item);
                    if (cell == null)
                    {
                        values = new();
                        return;
                    }
                    string value = await GetResult(sheetId, cell.Id, cell.Value);
                    values.Add(value);
                }
                else
                {
                    values.Add(item);
                }
            }
        }
        private string Calculate(string val1, string val2, char operation)
        {
            if (double.TryParse(val1, out double intFirstValue) && double.TryParse(val2, out double intSecondValue))
            {
                switch (operation)
                {
                    case '+':
                        return (intFirstValue + intSecondValue).ToString();
                    case '-':
                        return (intFirstValue - intSecondValue).ToString();
                    case '*':
                        return (intFirstValue * intSecondValue).ToString();
                    case '/':
                        return (intFirstValue / intSecondValue).ToString();
                    default:
                        return "Error";
                }
            }
            return "Error";
        }
        private void GetIddexesOfBreckets(List<int> openBrackets, List<int> closeBrackets, string cellValue)
        {
            for (int i = 0; i < cellValue.Length; i++)
            {
                char currentChar = cellValue[i];

                if (currentChar == '(')
                {
                    openBrackets.Add(i);
                }
                else if (currentChar == ')')
                {
                    closeBrackets.Add(i);
                }
            }

        }
    }
}
