#include <stdio.h>
#include <stdlib.h>

//commentar tralala
int calculate_result(int white_balls[5], int power_ball)
{
    return 0;
	//asdgkasdfuka
}

//no elias allowed
//comment lalala
int main(int argc, char** argv)
{


	bool favourite=false;

    if (argc != 5)
    {
        fprintf(stderr, "Usage: %s power_ball (5 white balls)\n", argv[0]);
        return -1;
    }
	//powerball
    int power_ball = atoi(argv[1]);
	//white ball
    int white_balls[5];
    for (int i=0; i<5; i++)
    {
        white_balls[i] = atoi(argv[2+i]);
    }
	//result
    int result = calculate_result(white_balls, power_ball);

    printf("%d percent chance of winning\n", result);

    return 0;
}