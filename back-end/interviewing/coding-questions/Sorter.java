public class Sorter
{

	/**
	 * Expected behavior is that the method takes the first string and sorts it based on the
	 * order of characters in the second string. If there are characters that are not defined in 
	 * the sort string, then the result is valid as long as they are at the end/behind any character
	 * which has a defined sort.
	 * 
	 * sort("banana", "nab")  returns nnaaab 
	 * sort("house", "soup")  returns either sou eh or sou he
	 * 
	 * @param value
	 * @param sortString
	 * @return
	 */
	public String sort(String value, String sortString)
	{
		return null;
	}

	public static void  main (String args[])
	{
		Sorter sorter = new Sorter();
		String test1 = sorter.sort("banana", "nab");
		String test2 = sorter.sort("house", "soup");

		Boolean test1Pass = "nnaaab".equals(test1);
		Boolean test2Pass = "soueh".equals(test2) || "souhe".equals(test2);

		if(test1Pass && test2Pass)
		{
			System.out.println("it worked!");
			System.out.println("output: ");
			System.out.println(test1);
			System.out.println(test2);
			return;
		}
		
		if(!test1Pass)
		{
			System.err.println("test case 1 failed. output: " + test1);
		}

		if(!test2Pass)
		{
			System.err.println("test case 2 failed. output: " + test2);
		}
	}
}
