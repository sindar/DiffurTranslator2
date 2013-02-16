function dxdt=t_funsys(t,x);
a=0.2;
b=0.25;
c=0.8;
d=1.22;

dxdt=zeros(3, 1);

dxdt(1)=x(3)*b + x(2)*d-x(1)*a-x(1)*c;
dxdt(2)=x(1)*c - x(2)*d;
dxdt(3)=x(1)*a - x(3)*b;

