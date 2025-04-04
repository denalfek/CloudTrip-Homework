# Use Node.js to build the Angular app
FROM node:20 AS build
WORKDIR /app

# Copy package.json and install dependencies
COPY cloudtrip.homework.client/package.json cloudtrip.homework.client/package-lock.json ./
RUN npm install

# Copy the entire Angular project
COPY cloudtrip.homework.client/ ./

# Build the Angular app in production mode
RUN npm run build -- --configuration=production

# Use Nginx for serving the built Angular app
FROM nginx:latest
COPY --from=build /app/dist/cloudtrip.homework.client /usr/share/nginx/html
COPY cloudtrip.homework.client/nginx/nginx.conf /etc/nginx/conf.d/default.conf 

# Expose port 90 for web traffic
EXPOSE 90
CMD ["nginx", "-g", "daemon off;"]
