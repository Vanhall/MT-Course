void main()
{
	int a;
	int b = 3;
	int s = b + 2;
	a = s + b - 7;

	while (a < b)
	{
		a = a + 1;
		while (s >= 3) s = s - a;
	}
}