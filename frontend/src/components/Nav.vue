<template>
  <nav>
    <img src="@/assets/logo.svg" alt="" />

    <section class="tabs">
      <div
        :class="['tooltip', { active: itemsActive }]"
        @click="changeTab('Items')"
      >
        <span class="material-icons"> list </span>
        <span class="tooltip-text">Items</span>
      </div>

      <div
        :class="['tooltip', 'middle-icon', { active: historyActive }]"
        @click="changeTab('History')"
      >
        <span class="material-icons"> loop </span>
        <span class="tooltip-text">History</span>
      </div>

      <div
        :class="['tooltip', { active: statisticsActive }]"
        @click="changeTab('Statistics')"
      >
        <span class="material-icons"> insert_chart_outlined </span>
        <span class="tooltip-text">Statistics</span>
      </div>
    </section>

    <div id="cart">
      <span class="material-icons"> shopping_cart </span>
      <span class="top-number">
        {{activeListLength}}
      </span>
    </div>
  </nav>
</template>

<script>
export default {
  name: "Nav",
  props: {
    activeListLength: {
      type: Number
    }
  },
  data() {
    return {
      itemsActive: true,
      historyActive: false,
      statisticsActive: false,
    };
  },
  methods: {
    changeTab(tab) {
      this.itemsActive = false;
      this.historyActive = false;
      this.statisticsActive = false;

      if (tab === "Items") this.itemsActive = true;
      if (tab === "History") this.historyActive = true;
      if (tab === "Statistics") this.statisticsActive = true;

      this.$emit("change-tab", tab);
    },
  },
};
</script>

<style scoped>
nav {
  min-height: 100vh;

  display: flex;
  align-items: center;
  flex-direction: column;
  justify-content: space-between;

  padding: 5rem 0;
}

nav .tabs {
  display: flex;
  align-items: center;
  flex-direction: column;
  width: 100%;
}

.tabs .tooltip {
  width: 100%;
  display: flex;
  position: relative;
  justify-content: center;
}

.tabs .tooltip.active::before {
  content: "";
  position: absolute;
  left: 0;

  height: 100%;
  width: 5px;
  background: #f9a109;
  border-radius: 0px 4px 4px 0px;
}

.tooltip .tooltip-text {
  opacity: 0;
  width: 60px;

  color: #fff;
  background: #454545;

  padding: 5px 0;
  border-radius: 4px;

  text-align: center;

  /* Position the tooltip */
  position: absolute;
  z-index: 1;

  top: 2px;
  left: 70%;

  transition: all 0.4s ease-out;
}

.tooltip:hover .tooltip-text {
  opacity: 1;
}

.tabs .material-icons {
  font-size: 2.8rem;

  cursor: pointer;
}

.tabs .middle-icon {
  margin: 6rem 0;
}

#cart {
  position: relative;
  padding: 5px;  
}

#cart .material-icons {
  color: white;
  background: #f9a109;

  padding: 1rem;
  border-radius: 50%;  
}

#cart .top-number {
  position: absolute;
  top: 0;
  right: 0;

  color: #ffffff;
  font-weight: 500;
  font-size: 1.3rem;

  border-radius: 4px;
  background: #eb5757;
  padding: 0.2rem 0.7rem;
}
</style>
