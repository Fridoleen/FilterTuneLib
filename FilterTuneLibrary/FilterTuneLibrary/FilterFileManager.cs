using System.Collections.Generic;
using System.IO;

namespace FilterTuneWPF
{
    /// <summary>
    /// Range of int numbers, used to describe parts of string array, that meet specified conditions
    /// </summary>
    struct Block
    {
        public int start;
        public int finish;

        public Block(int st, int fn)
        {
            this.start = st;
            this.finish = fn;
        }
    }

    class FilterFileManager
    {
        static string currentFilterPath;
        static string[] filterContent;

        public static void OpenFilter(string filePath)
        {
            FileInfo fileInf = new FileInfo(filePath);
            if(fileInf.Exists)
            {
                currentFilterPath = filePath;
            }
            filterContent = File.ReadAllLines(currentFilterPath);
            currentFilterPath = fileInf.DirectoryName;
        }        

        /// <summary>
        /// Applies template to the contents of chosen filter
        /// </summary>
        /// <param name="fTemplate"></param>
        public static void ApplyTemplate(FilterTemplate fTemplate)
        {
            List<Block> targetBlocks = new List<Block>(FindBlocks(0, fTemplate.Selectors));

            foreach (Block block in targetBlocks)
            {
                ReplaceValuesinBlock(block.start, block.finish, fTemplate.Parameters);
            }
        }

        /// <summary>
        /// Returns text blocks, that are situated after [start] line and contain selectors
        /// </summary>
        /// <param name="start"></param>
        /// <param name="selectors"></param>
        /// <returns></returns>
        private static List<Block> FindBlocks(int start, List<StringPair> selectors) // Done
        {            
            List<Block> foundBlockList = new List<Block>();

            for(int i = start; i < filterContent.Length; i++)
            {
                int foundShow = filterContent[i].IndexOf("Show");
                // int foundHide = filterContent[i].IndexOf("Hide");

                if(foundShow > -1)
                {                    
                    int lastFilterLine = filterContent.Length - 1;
                    int endBlock = lastFilterLine;

                    for (int j = 0; j < lastFilterLine - i; j++)
                    {
                        if(filterContent[i + j].Length < 3)
                        {
                            endBlock = i + j - 1;
                            break;
                        }

                        if (i + j >= lastFilterLine) break;
                    }

                    if (CheckBlock(i, endBlock, selectors))
                    {
                        Block blk = new Block(i, endBlock);
                        foundBlockList.Add(blk);
                    }

                    i = endBlock + 1;
                }
            }

            return foundBlockList;
        }

        /// <summary>
        /// Check if lines with numbers from [start] to [finish] contain selectors
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="selectors"></param>
        /// <returns></returns>
        private static bool CheckBlock(int start, int end, List<StringPair> selectors) 
        {
            bool result = true;

            foreach(StringPair sel in selectors)
            {
                bool selectorIsPresent = false;

                for(int i = start; i < end; i++)
                {
                    if (filterContent[i].IndexOf(sel.Name) > -1)
                    { 
                        if (filterContent[i].IndexOf(sel.Value) > -1)
                        { 
                            selectorIsPresent = true;
                        }                            
                        else break;
                    }
                        
                }

                result = result && selectorIsPresent;
            }

            return result;
        }

        /// <summary>
        /// Replaces parameter values for all parameters found in lines ranging numbers from start to finish
        /// </summary>
        /// <param name="start"></param>
        /// <param name="finish"></param>
        /// <param name="parameters"></param>
        private static void ReplaceValuesinBlock(int start, int finish, List<StringPair> parameters) 
        {
            for(int i = start; i < finish + 1; i++) 
            {
                foreach(StringPair par in parameters)
                {
                    int nameParameterPosition = filterContent[i].IndexOf(par.Name, 0);

                    if(nameParameterPosition > -1)
                    {                        
                        int commentaryPosition = filterContent[i].IndexOf('#', nameParameterPosition);      
                        int valueParameterPosition = nameParameterPosition + par.Name.Length + 1;

                        if (commentaryPosition > -1)                                              //if no comment in Line => erase all after parameter name and add parameter value
                        {
                            int commentSize = filterContent[i].Length - commentaryPosition;

                            filterContent[i] = filterContent[i].Remove(valueParameterPosition, commentaryPosition - valueParameterPosition);
                            filterContent[i].Insert(valueParameterPosition, par.Value + "\t");
                        }
                        else                                                         // else - erase from parameter name to comment beginning, add parameter
                        {
                            filterContent[i] = filterContent[i].Remove(valueParameterPosition);
                            filterContent[i] += par.Value;
                        }
                    }
                }
            }
        }

        public static void CreateNewFilter(string filterName) //check if result is correct
        {
            filterName += ".txt";
            File.WriteAllLines(currentFilterPath + @"\" + filterName, filterContent);
        }
    }
}
//In some cases BaseType parameter list is of the size of a MULTIPLE LINES - this may cause some problems
