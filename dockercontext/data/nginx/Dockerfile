FROM nginx:alpine AS builder

# nginx:alpine contains NGINX_VERSION environment variable, like so:
ENV NGINX_VERSION 1.25.4
# Our NCHAN version
ENV NGX_CACHE_PURGE 2.5.3

# Download sources
RUN curl "http://nginx.org/download/nginx-${NGINX_VERSION}.tar.gz" -o nginx.tar.gz && \
  curl -L "https://github.com/nginx-modules/ngx_cache_purge/archive/refs/tags/${NGX_CACHE_PURGE}.tar.gz" -o ngx-cache-purge.tar.gz

# For latest build deps, see https://github.com/nginxinc/docker-nginx/blob/master/mainline/alpine/Dockerfile
RUN apk add --no-cache --virtual .build-deps \
  gcc \
  libc-dev \
  make \
  openssl-dev \
  pcre-dev \
  zlib-dev \
  linux-headers \
  curl \
  gnupg \
  libxslt-dev \
  gd-dev \
  geoip-dev

# Reuse same cli arguments as the nginx:alpine image used to build
RUN CONFARGS=$(nginx -V 2>&1 | sed -n -e 's/^.*arguments: //p') \
    CONFARGS=${CONFARGS/-Os -fomit-frame-pointer -g/-Os} \
    mkdir /usr/src && \
	tar -zxC /usr/src -f nginx.tar.gz && \
  tar -xzvf "ngx-cache-purge.tar.gz" && \
  PURGEDIR="$(pwd)/ngx_cache_purge-${NGX_CACHE_PURGE}" && \
  cd /usr/src/nginx-$NGINX_VERSION && \
  ./configure --with-compat $CONFARGS --add-dynamic-module=$PURGEDIR && \
  make modules && \
  mv ./objs/*.so /


FROM nginx:alpine
# Extract the dynamic module NCHAN from the builder image
# COPY --from=builder /usr/local/nginx/modules/ngx_nchan_module.so /usr/local/nginx/modules/ngx_nchan_module.so
COPY --from=builder /ngx_http_cache_purge_module.so /etc/nginx/modules/ngx_http_cache_purge_module.so
RUN rm /etc/nginx/conf.d/default.conf

# COPY nginx.conf /etc/nginx/nginx.conf
# COPY default.conf /etc/nginx/conf.d/default.conf

COPY data/nginx/etc/nginx.conf /etc/nginx/nginx.conf

EXPOSE 85
STOPSIGNAL SIGTERM
CMD ["nginx", "-g", "daemon off;"]