function [ts,data]=t_euler()
x0=[0;0];
t0 =-20; dt = 0.25; tn = 20;
Nsteps = round(tn/dt)
ts = zeros(Nsteps,1)
data = zeros(Nsteps,length(x0))
ts(1) = t0
data(1,:) = x0'
for i =1:Nsteps
dxdt= feval(@t_funsys,t0,x0)
x0=x0+dxdt*dt
t0 = t0+dt
ts(i+1) = t0
data(i+1,:) = x0'
end
f = figure('Visible','off')
plot (ts,data(:,[1,2]),'lineWidth',3);
grid on
legend('x`1','x`2','x`3')
print('-dbmp','-r80','graf_eu.bmp')
end

