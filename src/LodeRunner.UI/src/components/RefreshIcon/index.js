import PropTypes from "prop-types";

const RefreshIcon = ({ height, arrowColor, backgroundColor }) => (
  <svg
    version="1.1"
    id="Layer_1"
    xmlns="http://www.w3.org/2000/svg"
    x="0px"
    y="0px"
    height={height}
    width={height}
    viewBox="0 0 496.166 496.166"
  >
    <path
      fill={arrowColor}
      d="M0.005,248.087C0.005,111.063,111.073,0,248.079,0c137.014,0,248.082,111.062,248.082,248.087 c0,137.002-111.068,248.079-248.082,248.079C111.073,496.166,0.005,385.089,0.005,248.087z"
    />
    <path
      fill={backgroundColor}
      d="M400.813,169.581c-2.502-4.865-14.695-16.012-35.262-5.891 c-20.564,10.122-10.625,32.351-10.625,32.351c7.666,15.722,11.98,33.371,11.98,52.046c0,65.622-53.201,118.824-118.828,118.824 c-65.619,0-118.82-53.202-118.82-118.824c0-61.422,46.6-111.946,106.357-118.173v30.793c0,0-0.084,1.836,1.828,2.999 c1.906,1.163,3.818,0,3.818,0l98.576-58.083c0,0,2.211-1.162,2.211-3.436c0-1.873-2.211-3.205-2.211-3.205l-98.248-57.754 c0,0-2.24-1.605-4.23-0.826c-1.988,0.773-1.744,3.481-1.744,3.481v32.993c-88.998,6.392-159.23,80.563-159.23,171.21 c0,94.824,76.873,171.696,171.693,171.696c94.828,0,171.707-76.872,171.707-171.696 C419.786,219.788,412.933,193.106,400.813,169.581z"
    />
  </svg>
);

RefreshIcon.propTypes = {
  height: PropTypes.string,
  arrowColor: PropTypes.string,
  backgroundColor: PropTypes.string,
};

RefreshIcon.defaultProps = {
  height: "1em",
  arrowColor: "#32BEA6",
  backgroundColor: "#f7f7f7",
};

export default RefreshIcon;
