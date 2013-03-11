function t_ode45()
tspan=[-20:0.25:20]
x0=[0;0];
[t,x]=ode45(@t_funsys,tspan,x0);
f = figure('Visible','off')
plot (t,x(:,[1,2]),'lineWidth',3);
grid on
legend('x`1','x`2','x`3')
print('-dbmp','-r80','graf_ode45.bmp')

