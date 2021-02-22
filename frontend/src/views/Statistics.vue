<template>
  <div id="statistics-tab">
    <section class="top-info">
      <div class="top-items">
        <h2>Top items</h2>

        <article class="top-item" v-for="item in topItems" :key="item.label">
          <p>
            <span>{{item.label}}</span>
            <span class="porcent">{{Math.round(item.value)}}%</span>
          </p>

          <div class="progress-bar">
            <div class="fill-item" :style="'--width: '+item.value+'%;'"></div>
          </div>
        </article>

      </div>

      <div class="top-categories">
        <h2>Top Categories</h2>

        <article class="top-category" v-for="category in topCategories" :key="category.label">
          <p>
            <span>{{category.label}}</span>
            <span class="porcent">{{Math.round(category.value)}}%</span>
          </p>

          <div class="progress-bar">
            <div class="fill-category" :style="'--width: '+category.value+'%;'"></div>
          </div>
        </article>        
      </div>
    </section>

    <section id="chart-container">
      <h2>Monthly Summary</h2>

      <apexcharts      
        width="100%"
        height="70%"
        type="line"
        :options="options"
        :series="series"
      ></apexcharts>
    </section>
  </div>
</template>

<script>
import VueApexCharts from "vue3-apexcharts";

export default {
  name: "Statistics",
  components: {
    apexcharts: VueApexCharts,
  },
  props: {  
    topItems: {
      type: Array,
    },
    topCategories: {
      type: Array,
    },
    monthlyData: {
      type: Array,
    },
    monthlyCategories: {
      type: Array,
    },
  },
  data() {
    return {
      options: {
        chart: {
          id: "basic-bar",
          fontFamily: "Quicksand, sans-serif",
          toolbar: {
            show: false,
          },
          zoom: {
            enabled: false,
          },
        },
        grid: {
          borderColor: "#C8C8C8",
          strokeDashArray: 5,
          xaxis: {
            lines: {
              show: true,
            },
          },
          yaxis: {
            lines: {
              show: true,
            },
          },
        },
        xaxis: {
          categories: this.monthlyCategories,
        },
        colors: ["#F9A109"],
        stroke: {
          curve: "smooth",
          lineCap: "round",
          width: 2,
        },
        markers: {
          size: 4,
          shape: "circle",
          colors: "#fff",
          strokeColors: "#F9A109",
        },        
      },
      series: [
        {
          name: "Items",
          data: this.monthlyData,
        },
      ],
    };
  },
};
</script>

<style scoped>
#statistics-tab {
  display: grid;
  row-gap: 3rem;
  padding: 5rem 12rem;
  grid-template-rows: auto 1fr;
  grid-template-columns: 1fr;
}

.top-info {
  display: flex;
  grid-row: 1 / 2;
  column-gap: 6.5rem;
}

.top-items,
.top-categories {
  flex-grow: 1;
}

.top-items h2,
.top-categories h2 {
  font-weight: 500;
  font-size: 2.4rem;
  margin-bottom: 4rem;
}

.top-item,
.top-category {
  margin-bottom: 3rem;
}

.top-item p,
.top-category p {
  font-weight: 500;
  font-size: 1.4rem;

  display: flex;
  align-items: center;
  justify-content: space-between;

  margin-bottom: 1.5rem;
}

.top-item p .porcent,
.top-category p .porcent {
  font-size: 1.8rem;
}

.top-item .progress-bar,
.top-category .progress-bar {
  height: 0.8rem;
  border-radius: 4px;
  background: #e0e0e0;
}

.fill-item,
.fill-category {
  width: var(--width);
  height: 100%;
  border-radius: 4px;
}

.fill-item {
  background: #f9a109;
}

.fill-category {
  background: #56ccf2;
}

#chart-container {
  grid-row: 2 / 3;
  height: 100%;
}

#chart-container h2 {
  font-weight: 500;
  font-size: 2.4rem;
  margin-bottom: 4rem;
}
</style>
